using EngineProject.DataStructures;
using EngineProject.Engines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineProject
{
    public class EngineComponent : IEngineComponent
    {
        private IEngine _engine;

        public void CreateEngine(EngineType type,int width, int height)
        {
            switch (type)
            {
                case EngineType.OneDimensionEngine:
                    _engine = new OneDimensionEngine(width, height);
                    break;
                default:
                    throw new NotSupportedException("Unrecognized Engine type");
            }
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
        public void ChangeCellState(int x, int y)
        {
            _engine.ChangeCellState(x, y);
        }
    }
}
