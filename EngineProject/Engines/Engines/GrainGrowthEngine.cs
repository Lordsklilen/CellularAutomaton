using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineProject.DataStructures;

namespace EngineProject.Engines.Engines
{
    public class GrainGrowthEngine : IEngine
    {
        public Board panel { get; private set; }
        public EngineType type;
        private int _maxRow;
        private int _maxColumn;

        public GrainGrowthEngine(int width, int height)
        {
            panel = new Board(width, height);
            type = EngineType.GrainGrowth;
            _maxRow = height;
            _maxColumn = width;
        }

        public void ChangeCellState(int x, int y)
        {
            throw new NotImplementedException();
        }

        public Board GetBoard()
        {
            throw new NotImplementedException();
        }

        public void NextIteration()
        {
            throw new NotImplementedException();
        }

        public void SetCellState(int x, int y, bool state)
        {
            throw new NotImplementedException();
        }

        public void SetRule(int rule)
        {
            throw new NotImplementedException();
        }
    }
}
