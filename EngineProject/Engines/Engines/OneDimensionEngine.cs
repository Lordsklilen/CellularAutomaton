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
        private int[] weights;
        public OneDimensionEngine(int width, int height)
        {
            panel = new Board(width, height);
            type = EngineType.OneDimensionEngine;
            _createdRows = 0;
            _maxRow = height;
            _maxColumn = width;
            _rule = 90;
            weights = new int[8];
            SetRule(_rule);
        }

        public Board GetBoard()
        {
            return panel;
        }

        public void NextIteration()
        {
            if (_maxRow <= _createdRows + 1)
            {
                _createdRows = 0;
                return;
            }
            for (int i = 0; i < _maxColumn; i++)
            {
                CheckNeighbours(i);
            }
            _createdRows++;
        }

        public void ChangeCellState(int x, int y)
        {
            if (x > 0)
                return;
            panel.SetCellState(x, y, !panel.board[x][y].GetState());
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
            panel.SetCellState(x, y, state);
        }

        private void CheckNeighbours(int i)
        {
            int left = panel.board[_createdRows][(i + _maxColumn - 1) % _maxColumn].GetState() ? 4 : 0;
            int middle = panel.board[_createdRows][i].GetState() ? 2 : 0;
            int right = panel.board[_createdRows][(i + 1 + _maxColumn) % _maxColumn].GetState() ? 1 : 0;
            panel.SetCellState(_createdRows + 1, i, weights[left + middle + right] == 1 ? true : false);
        }

        private void ComputeWeights(int rule)
        {
            for (int i = 0; i < 8; i++)
            {
                weights[i] = rule % 2;
                rule /= 2;
            }
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
