using EngineProject.DataStructures;
using EngineProject.Engines.DRX;
using EngineProject.Engines.MonteCarlo;
using EngineProject.Engines.NeighbourStrategy;
using EngineProject.Templates.GrainTemplates;

namespace EngineProject
{
    public interface IEngineComponent
    {
        int MaxNumber { get; }
        bool IsFinished { get; }
        Board Board { get; }
        void CreateEngine(EngineType type,int width, int height);
        Board GetNextIteration();
        void SetCellState(int x, int y, bool state);
        void ChangeCellState(int x, int y);
        void SetRule(int rule);
        void SetGrainNumber(int grainNumber, int x, int y);
        void GenerateGrainTemplate(TemplateRequest request);
        void ChangeBorderConditions(bool state);
        void ChangeNeighboroodType(NeighbourStrategyRequest request);
        void CalculateMonteCarlo(MonteCarloRequest request);
        void CalculateEnergy(MonteCarloRequest request);
        Board CalculateDRX(DRXRequest request);
    }
}
