using System;
using System.Collections.Generic;
using System.Linq;
using EngineProject.DataStructures;

namespace EngineProject.Engines.NeighbourStrategy
{
    public class NeighbourVonNeumann : INeighbourStrategy
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
            int[,] pairs = { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };

            for (int i = 0; i <= 3; i++)
            {
                int x = pairs[i, 0];
                int y = pairs[i, 1];
                int widthId = 0;
                int heightId = 0;
                int number = 0;
                if (OpenBorderCondition)
                {
                    widthId = (x + cell.x) >= 0 ? (x + cell.x) % (_maxRow) : _maxRow - 1;
                    heightId = (y + cell.y) >= 0 ? (y + cell.y) % (_maxColumn) : _maxColumn - 1;
                    number = ((Grain)panel.board[widthId][heightId]).GetGrainNumber();
                }
                else
                {
                    widthId = (x + cell.x);
                    heightId = (y + cell.y);

                    if (widthId < 0 || heightId < 0 || widthId >= _maxRow || heightId >= _maxColumn)
                        number = 0;
                    else
                        number = ((Grain)panel.board[widthId][heightId]).GetGrainNumber();
                }

                if (number > 0)
                    neighbours.Add(number);
            }
            return neighbours;
        }
    }
}
