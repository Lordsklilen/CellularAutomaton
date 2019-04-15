using EngineProject.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineProject.Engines
{
    public class OneDimensionEngine : IEngine
    {
        public Board panel { get; private set; }
        public EngineType type;
        private int _createdRows;
        private int _maxRow;
        private int _maxColumn;
        private int _rule;

        public OneDimensionEngine(int width, int height)
        {
            panel = new Board(width, height);
            type = EngineType.OneDimensionEngine;
            _createdRows = 0;
            _maxRow = height;
            _maxColumn = width;
            _rule = 90; // TODO
        }

        public Board GetBoard()
        {
            return panel;
        }

        public void NextIteration()
        {
            if (_maxRow <= _createdRows)
                return;
            for (int i = 1; i < _maxColumn - 1; i++)
            {
                CheckNeighbours(i);
            }
            _createdRows++;
        }

        public void ChangeCellState(int x, int y)
        {
            if (x > 0)
                return;
            panel.SetCellState(x, y, !panel.board[x][y].state);
        }
        public void SetCellState(int x, int y, bool state)
        {
            if (x > 0)
                return;
            panel.SetCellState(x, y, state);
        }
        private void CheckNeighbours(int i)
        {
            Cell left = panel.board[_createdRows][i - 1];
            Cell middle = panel.board[_createdRows][i];
            Cell Right = panel.board[_createdRows][i + 1];
            //middle.state = true;//TODO
        }
    }
}
