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
        private int _maxRow;
        private int _maxColumn;
        private bool OpenBorderCondition;

        public Board CopyPanel => copyPanel;

        public void Initialize(Board panel, Board copyPanel, int _maxRow, int _maxColumn, bool OpenBorderCondition)
        {
            this.panel = panel;
            this.copyPanel = copyPanel;
            this._maxRow = _maxRow;
            this._maxColumn = _maxColumn;
            this.OpenBorderCondition = OpenBorderCondition;
        }

        public void ComputeCell(Grain cell)
        {
            if (cell.GetGrainNumber() == 0)
            {
                var neighbours = NeighboursGrainNumbers(cell as Grain);
                copyPanel.SetGrainNumber(Utils.MostCommonNeighbour(neighbours), cell.x, cell.y);
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
                    int number = 0;
                    int widthId;
                    int heightId;
                    if (OpenBorderCondition)
                    {
                        widthId = (i + cell.x) >= 0 ? (i + cell.x) % (_maxRow) : _maxRow - 1;
                        heightId = (j + cell.y) >= 0 ? (j + cell.y) % (_maxColumn) : _maxColumn - 1;
                        number = ((Grain)panel.board[widthId][heightId]).GetGrainNumber();
                    }
                    else
                    {
                        widthId = (i + cell.x);
                        heightId = (j + cell.y);

                        if (widthId < 0 || heightId < 0 || widthId >= _maxRow || heightId >= _maxColumn)
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
