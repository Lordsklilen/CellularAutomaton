using EngineProject.DataStructures;
using EngineProject.Engines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineProject
{
    public class EngineFacade
    {
        private IEngine _engine;

        public void Create1DCellularAutomation(int width, int height)
        {
            _engine = new CellularAutomation1D(width, height);
        }

        public Board GetNextIteration()
        {
            _engine.NextIteration();
            return _engine.GetBoard();
        }
        public Board GetBoard()
        {
            return _engine.GetBoard();
        }
        public void SetCellState(int x, int y, bool state)
        {
            _engine.SetCellState(x, y, state);

        }
        public void ChangeCellState(int x, int y, bool state)
        {
        }
    }
}
