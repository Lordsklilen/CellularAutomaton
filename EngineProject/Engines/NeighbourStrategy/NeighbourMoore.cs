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
                var neighbours = NeighboursGrainNumbers(cell as Grain);
                copyPanel.SetGrainNumber(NeighbourHelper.MostCommonNeighbour(neighbours), cell.x, cell.y);
                copyPanel.finished = false;
            }
            else
                copyPanel.SetGrainNumber(cell.GetGrainNumber(), cell.x, cell.y);
        }

        public List<int> NeighboursGrainNumbers(Grain cell)
        {
            List<int> neighbours = new List<int>();

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                        continue;
                    int number = 0;
                    int widthId;
                    int heightId;
                    if (OpenBorderCondition)
                    {
                        widthId = (i + cell.x) >= 0 ? (i + cell.x) % (maxRow) : (i + cell.x)+ maxRow;
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
                        neighbours.Add(number);
                }
            }
            return neighbours;
        }
    }
}
