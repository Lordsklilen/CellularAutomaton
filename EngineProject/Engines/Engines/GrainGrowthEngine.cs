using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineProject.DataStructures;
using EngineProject.DataStructures.interfaces;

namespace EngineProject.Engines.Engines
{
    public class GrainGrowthEngine : IEngine
    {
        public Board panel { get; private set; }
        public EngineType type;
        private CellType cellType;
        private int _maxRow;
        private int _maxColumn;

        public Board GetBoard() => panel;

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
            var copyPanel = new Board(_maxColumn, _maxRow);
            foreach (var row in panel.board)
            {
                foreach (var cell in row)
                {
                    ComputeCell(cell, copyPanel);
                }

            }
            panel = copyPanel;
        }

        private void ComputeCell(ICell cell, Board copyPanel)
        {
            int neighbours = MaxNeighbours(cell);
            //if (cell.state)
            //{
            //    if (neighbours == 2 || neighbours == 3)
            //        copyPanel.board[cell.x][cell.y].state = true;
            //    else
            //        copyPanel.board[cell.x][cell.y].state = false;
            //}
            //else
            //{
            //    if (neighbours == 3)
            //        copyPanel.board[cell.x][cell.y].state = true;
            //    else
            //        copyPanel.board[cell.x][cell.y].state = false;
            //}
        }
        public void ChangeCellState(int x, int y)
        {
            panel.SetCellState(x, y, !panel.board[x][y].GetState());
        }

        public void SetCellState(int x, int y, bool state)
        {
            panel.SetCellState(x, y, state);
        }

        private int MaxNeighbours(ICell cell)
        {
            //int counter = 0;
            //for (int i = -1; i <= 1; i++)
            //{
            //    for (int j = -1; j <= 1; j++)
            //    {
            //        int widthId = (i + cell.x) >= 0 ? (i + cell.x) % (_maxRow) : _maxRow - 1;
            //        int heightId = (j + cell.y) >= 0 ? (j + cell.y) % (_maxColumn) : _maxColumn - 1;

            //        if (panel.board[widthId][heightId].state && !(i == 0 && j == 0))
            //            counter++;
            //    }
            //}
            return 0;
        }
        public void SetRule(int rule)
        {
            throw new NotImplementedException();
        }
    }
}
