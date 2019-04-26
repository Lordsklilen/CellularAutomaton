using EngineProject.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineProject.Engines
{
    public class GameOfLifeEngine : IEngine
    {
        public Board panel { get; private set; }
        public EngineType type;
        private int _maxRow;
        private int _maxColumn;
        public GameOfLifeEngine(int width, int height)
        {
            panel = new Board(width, height);
            type = EngineType.GameOfLife;
            _maxRow = height;
            _maxColumn = width;
        }

        public Board GetBoard()
        {
            return panel;
        }

        public void NextIteration()
        {
            var copyPanel = new Board(_maxColumn,_maxRow);
            foreach (var row in panel.board)
            {
                foreach (var cell in row)
                {
                    ComputeCell(cell, copyPanel);
                }

            }
            panel = copyPanel;
        }

        private void ComputeCell(Cell cell, Board copyPanel)
        {
            int neighbours = CheckNeighbours(cell);
            if (cell.state)
            {
                if (neighbours == 2 || neighbours == 3)
                    copyPanel.board[cell.x][cell.y].state = true;
                else
                    copyPanel.board[cell.x][cell.y].state = false;
            }
            else
            {
                if (neighbours == 3)
                    copyPanel.board[cell.x][cell.y].state = true;
                else
                    copyPanel.board[cell.x][cell.y].state = false;
            }
        }
        public void ChangeCellState(int x, int y)
        {
            panel.SetCellState(x, y, !panel.board[x][y].state);
        }

        public void SetCellState(int x, int y, bool state)
        {
            panel.SetCellState(x, y, state);
        }

        private int CheckNeighbours(Cell cell)
        {
            //TODO
            int counter = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int widthId = (i + cell.x) >= 0 ? (i + cell.x) % (_maxRow) : _maxRow - 1;
                    int heightId = (j + cell.y) >= 0 ? (j + cell.y) % (_maxColumn) : _maxColumn - 1;

                    if (panel.board[widthId][heightId].state && !(i == 0 && j == 0))
                        counter++;
                }
            }
            return counter;
        }
        public void SetRule(int rule)
        {
            throw new NotImplementedException();
        }
    }
}
