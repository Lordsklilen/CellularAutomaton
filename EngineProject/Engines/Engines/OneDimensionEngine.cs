using EngineProject.DataStructures;
using System;

namespace EngineProject.Engines
{
    public class OneDimensionEngine : IEngine
    {
        public Board Panel { get; private set; }
        public EngineType type;
        private int _createdRows;
        private readonly int _maxRow;
        private readonly int _maxColumn;
        private int _rule;
        private readonly int[] weights;
        public OneDimensionEngine(int width, int height)
        {
            Panel = new Board(width, height);
            type = EngineType.OneDimensionEngine;
            _createdRows = 0;
            _maxRow = height;
            _maxColumn = width;
            _rule = 90;
            weights = new int[8];
            SetRule(_rule);
        }

        public Board NextIteration()
        {
            if (_maxRow <= _createdRows + 1)
            {
                _createdRows = 0;
                return Panel;
            }
            for (int i = 0; i < _maxColumn; i++)
            {
                CheckNeighbours(i);
            }
            _createdRows++;
            return Panel;
        }

        public void ChangeCellState(int x, int y)
        {
            if (x > 0)
                return;
            Panel.SetCellState(x, y, !Panel.BoardContainer[x][y].GetState());
        }

        public void SetRule(int rule)
        {
            if (rule > 255 || rule < 0)
                throw new NotSupportedException("Rule is cannot be computed. Try number between 0-255");
            _rule = rule;
            ComputeWeights(_rule);
        }

        public void SetCellState(int x, int y, bool state)
        {
            if (x > 0)
                return;
            Panel.SetCellState(x, y, state);
        }

        private void CheckNeighbours(int i)
        {
            int left = Panel.BoardContainer[_createdRows][(i + _maxColumn - 1) % _maxColumn].GetState() ? 4 : 0;
            int middle = Panel.BoardContainer[_createdRows][i].GetState() ? 2 : 0;
            int right = Panel.BoardContainer[_createdRows][(i + 1 + _maxColumn) % _maxColumn].GetState() ? 1 : 0;
            Panel.SetCellState(_createdRows + 1, i, weights[left + middle + right] == 1);
        }

        private void ComputeWeights(int rule)
        {
            for (int i = 0; i < 8; i++)
            {
                weights[i] = rule % 2;
                rule /= 2;
            }
        }
    }
}
