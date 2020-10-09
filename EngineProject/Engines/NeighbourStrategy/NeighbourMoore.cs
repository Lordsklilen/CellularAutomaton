using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineProject.DataStructures;

namespace EngineProject.Engines.NeighbourStrategy
{
    public class NeighbourMoore : INeighbourStrategy
    {
        private Board panel;
        private Board copyPanel;
        private int maxRow;
        private int maxColumn;
        private bool OpenBorderCondition;

        public Board CopyPanel => copyPanel;
        public int N => 8;

        public void Initialize(Board panel, Board copyPanel, int _maxRow, int _maxColumn, bool OpenBorderCondition)
        {
            this.panel = panel;
            this.copyPanel = copyPanel;
            this.maxRow = _maxRow;
            this.maxColumn = _maxColumn;
            this.OpenBorderCondition = OpenBorderCondition;
        }

        public void ComputeCell(Grain cell)
        {
            if (cell.GetGrainNumber() == 0)
            {
                var neighbours = NeighboursGrainNumbers(cell);
                copyPanel.SetGrainNumber(NeighbourHelper.MostCommonNeighbour(neighbours), cell.x, cell.y);
                copyPanel.finished = false;
            }
            else
                copyPanel.SetGrainNumber(cell.GetGrainNumber(), cell.x, cell.y);
        }

        public List<int> NeighboursGrainNumbers(Grain cell)
        {
            return GetOnlyGrainNumbers(NeighboursGrainCells(cell));
        }

        public List<int> GetOnlyGrainNumbers(List<Grain> cells)
        {
            return cells.Select(x => x.GetGrainNumber()).ToList();
        }

        public List<int> GetOnlyRecrystalizationNumbers(List<Grain> cells)
        {
            return cells.Select(x => x.RecrystalizedNumber).ToList();
        }

        public int GetRecrystalizedAndGrainGrains(List<Grain> grains, int recrystalizationNumber, int grainNumber)
        {
            return grains.Count(x => (x.GetGrainNumber() != 0 && x.GetGrainNumber() != grainNumber) ||
                (x.RecrystalizedNumber != 0 && x.RecrystalizedNumber != recrystalizationNumber));
        }

        public List<Grain> NeighboursGrainCells(Grain cell)
        {
            List<Grain> neighbours = new List<Grain>();

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                        continue;
                    int widthId;
                    int heightId;
                    int number;
                    if (OpenBorderCondition)
                    {
                        widthId = (i + cell.x) >= 0 ? (i + cell.x) % (maxRow) : (i + cell.x) + maxRow;
                        heightId = (j + cell.y) >= 0 ? (j + cell.y) % (maxColumn) : (j + cell.y) + maxColumn;
                        number = ((Grain)panel.board[widthId][heightId]).GetGrainNumber();
                    }
                    else
                    {
                        widthId = (i + cell.x);
                        heightId = (j + cell.y);

                        if (widthId < 0 || heightId < 0 || widthId >= maxRow || heightId >= maxColumn)
                            number = 0;
                        else
                            number = ((Grain)panel.board[widthId][heightId]).GetGrainNumber();
                    }

                    if (number > 0)
                        neighbours.Add(((Grain)panel.board[widthId][heightId]));
                }
            }
            return neighbours;
        }

    }
}
