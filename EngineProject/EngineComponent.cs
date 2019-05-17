using EngineProject.DataStructures;
using EngineProject.Engines;
using EngineProject.Engines.Engines;
using EngineProject.Templates.GrainTemplates;
using System;

namespace EngineProject
{
    public class EngineComponent : IEngineComponent
    {
        private IEngine _engine;
        private GrainTemplateFactory templateFactory;

        public int MaxNumber =>_engine.GetBoard().MaxNumber();
        public bool IsFinished => (_engine as GrainGrowthEngine).IsFinished() || MaxNumber == 1;

        public Board Board => _engine.GetBoard();

        public void CreateEngine(EngineType type, int width, int height)
        {
            switch (type)
            {
                case EngineType.OneDimensionEngine:
                    _engine = new OneDimensionEngine(width, height);
                    break;
                case EngineType.GameOfLife:
                    _engine = new GameOfLifeEngine(width, height);
                    break;
                case EngineType.GrainGrowth:
                    _engine = new GrainGrowthEngine(width, height);
                    templateFactory = new GrainTemplateFactory();
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

        public void SetCellState(int x, int y, bool state)
        {
            _engine.SetCellState(x, y, state);
        }

        public void ChangeCellState(int x, int y)
        {
            _engine.ChangeCellState(x, y);
        }

        public void SetRule(int rule)
        {
            _engine.SetRule(rule);
        }
        public void SetGrainNumber(int grainNumber, int x, int y)
        {
            (_engine as GrainGrowthEngine).SetGrainNumber(grainNumber, x, y);
        }

        public void GenerateGrainTemplate(TemplateRequest request)
        {
            var template = templateFactory.CreateTemplate(request.type);
            template.GenerateTemplate(request);
        }

        public void ChangeBorderConditions(bool state)
        {
            _engine.ChangeBorderConditions(state);
        }

        public void ChangeNeighbooroodType(NeighbooorhoodType type)
        {
           //TODO
        }
    }
}
