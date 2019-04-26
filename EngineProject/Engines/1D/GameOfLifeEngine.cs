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
            //TODO
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
           //TODO
        }

        public void SetRule(int rule)
        {
            throw new NotImplementedException();
        }
    }
}
