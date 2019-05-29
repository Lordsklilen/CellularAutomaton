using EngineProject.DataStructures;
using EngineProject.Engines;
using EngineProject.Engines.Engines;
using EngineProject.Engines.MonteCarlo;
using EngineProject.Engines.NeighbourStrategy;
using EngineProject.Templates.GrainTemplates;
using System;

namespace EngineProject
{
    public class EngineComponent : IEngineComponent
    {
        private IEngine engine;
        private GrainTemplateFactory templateFactory;

        public int MaxNumber =>engine.GetBoard().MaxNumber();
        public bool IsFinished => (engine as GrainGrowthEngine).IsFinished() || MaxNumber == 0;

        public Board Board => engine.GetBoard();

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
                case EngineType.GrainGrowth:
                    engine = new GrainGrowthEngine(width, height);
                    templateFactory = new GrainTemplateFactory();
                    break;
                default:
                    throw new NotSupportedException("Unrecognized Engine type");
            }
        }

        public Board GetNextIteration()
        {
            engine.NextIteration();
            return engine.GetBoard();
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
            engine.SetRule(rule);
        }
        public void SetGrainNumber(int grainNumber, int x, int y)
        {
            (engine as GrainGrowthEngine).SetGrainNumber(grainNumber, x, y);
            //(engine as GrainGrowthEngine).RecalculateEnergy();
        }

        public void GenerateGrainTemplate(TemplateRequest request)
        {
            var template = templateFactory.CreateTemplate(request.type);
            template.GenerateTemplate(request);
            (engine as GrainGrowthEngine).RecalculateEnergy();
        }

        public void ChangeBorderConditions(bool state)
        {
            engine.ChangeBorderConditions(state);
        }

        public void ChangeNeighboroodType(NeighbourStrategyRequest request)
        {
            (engine as GrainGrowthEngine).ChangeStrategyType(request);
        }

        public void CalculateMonteCarlo(MonteCarloRequest request)
        {
            (engine as GrainGrowthEngine).CreateMCEngine(request);
            (engine as GrainGrowthEngine).IterateMonteCarlo(request.numberOfIterations);

        }

        public void CalculateEnergy(MonteCarloRequest request)
        {
            (engine as GrainGrowthEngine).CreateMCEngine(request);
        }
    }
}
