using System;
using System.Collections.Generic;
using System.Linq;
using EngineProject.DataStructures;

namespace EngineProject.Engines.Engines
{
    public class GrainGrowthEngine : IEngine
    {
        public Board panel { get; private set; }
        public EngineType type;
        private CellType cellType;
        private int _maxRow;
        private int _maxColumn;
        private bool OpenBorderCondition = true;
    
        public Board GetBoard() => panel;
        public bool IsFinished() => panel.IsFinished();
        public GrainGrowthEngine(int width, int height)
        {
            type = EngineType.GrainGrowth;
            cellType = CellType.Grain;
            panel = new Board(width, height, cellType);
            _maxRow = height;
            _maxColumn = width;
        }

        public void NextIteration()
        {
            var copyPanel = new Board(_maxColumn, _maxRow, cellType);
            copyPanel.finished = true;
            foreach (var row in panel.board)
            {
                foreach (var cell in row)
                {
                    ComputeCell((Grain)cell, copyPanel);
                }
            }
            panel = copyPanel;
        }

        private void ComputeCell(Grain cell, Board copyPanel)
        {
            if (cell.GetGrainNumber() == 0)
            {
                int nextGrainNumber = MostCommonNeighbour(cell as Grain);
                copyPanel.SetGrainNumber(nextGrainNumber, cell.x, cell.y);
                copyPanel.finished = false;
            }
            else
                copyPanel.SetGrainNumber(cell.GetGrainNumber(), cell.x, cell.y);
        }
        public void ChangeCellState(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void SetCellState(int x, int y, bool state)
        {
            throw new NotImplementedException();
        }

        private int MostCommonNeighbour(Grain cell)
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
                else {
                    widthId = (x + cell.x);// >= 0 ? (x + cell.x) % (_maxRow) : _maxRow - 1;
                    heightId = (y + cell.y);// >= 0 ? (y + cell.y) % (_maxColumn) : _maxColumn - 1;

                    if (widthId < 0 || heightId < 0 || widthId >= _maxRow || heightId >= _maxColumn)
                        number = 0;
                    else 
                        number = ((Grain)panel.board[widthId][heightId]).GetGrainNumber();
                }

                if (number > 0)
                    neighbours.Add(number);
            }
            if (neighbours.Count == 0)
                return 0;
            else
            {
                var groups = neighbours.GroupBy(x => x);
                return groups.OrderByDescending(x => x.Count()).First().Key;
            }
        }
        public void SetRule(int rule)
        {
            throw new NotImplementedException();
        }

        public void SetGrainNumber(int number, int x, int y)
        {
            panel.SetGrainNumber(number, x, y);
        }

        public void ChangeBorderConditions(bool state)
        {
            OpenBorderCondition = state;
        }
    }
}
