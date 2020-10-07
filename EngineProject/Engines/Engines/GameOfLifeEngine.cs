using EngineProject.DataStructures;
using System;

namespace EngineProject.Engines
{
    public class GameOfLifeEngine : IEngine
    {
        public Board panel { get; private set; }
        public EngineType type;
        private readonly int _maxRow;
        private readonly int _maxColumn;

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
            var copyPanel = new Board(panel);
            foreach (var row in panel.board)
            {
                foreach (var cell in row)
                {
                    ComputeCell(cell as Cell, copyPanel);
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
                    copyPanel.board[cell.x][cell.y].SetState(true);
                else
                    copyPanel.board[cell.x][cell.y].SetState(false);
            }
            else
            {
                if (neighbours == 3)
                    copyPanel.board[cell.x][cell.y].SetState(true);
                else
                    copyPanel.board[cell.x][cell.y].SetState(false);
            }
        }
        public void ChangeCellState(int x, int y)
        {
            panel.SetCellState(x, y, !panel.board[x][y].GetState());
        }

        public void SetCellState(int x, int y, bool state)
        {
            panel.SetCellState(x, y, state);
        }

        private int CheckNeighbours(Cell cell)
        {
            int counter = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int widthId = (i + cell.x) >= 0 ? (i + cell.x) % (_maxRow) : _maxRow - 1;
                    int heightId = (j + cell.y) >= 0 ? (j + cell.y) % (_maxColumn) : _maxColumn - 1;

                    if (panel.board[widthId][heightId].GetState() && !(i == 0 && j == 0))
                        counter++;
                }
            }
            return counter;
        }

        public void SetRule(int rule)
        {
            throw new NotImplementedException();
        }

        public void SetGrainNumber(int number, int x, int y)
        {
            throw new NotImplementedException();
        }

        public void ChangeBorderConditions(bool state)
        {
            throw new NotImplementedException();
        }
    }
}
