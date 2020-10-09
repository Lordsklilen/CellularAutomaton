using EngineProject.DataStructures;
using EngineProject.Engines;
using EngineProject.Engines.Engines;
using System;

namespace EngineProject
{
    public class EngineComponent : IEngineComponent
    {
        private IEngine engine;
        public int MaxNumber => engine.Panel.MaxNumber();
        public bool IsFinished => (engine as GrainGrowthEngine).IsFinished || MaxNumber == 0;
        public Board Panel => engine.Panel;

        public void CreateEngine(EngineType type, int width, int height)
        {
            switch (type)
            {
                case EngineType.OneDimensionEngine:
                    engine = new OneDimensionEngine(width, height);
                    break;
                case EngineType.GameOfLife:
                    engine = new GameOfLifeEngine(width, height);
                    break;
                default:
                    throw new NotSupportedException("Unrecognized Engine type");
            }
        }

        public Board GetNextIteration()
        {
            engine.NextIteration();
            return engine.Panel;
        }

        public void SetCellState(int x, int y, bool state)
        {
            engine.SetCellState(x, y, state);
        }

        public void ChangeCellState(int x, int y)
        {
            engine.ChangeCellState(x, y);
        }

        public void SetRule(int rule)
        {
            (engine as OneDimensionEngine).SetRule(rule);
        }
    }
}
