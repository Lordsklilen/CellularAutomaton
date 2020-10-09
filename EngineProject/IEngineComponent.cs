using EngineProject.DataStructures;

namespace EngineProject
{
    public interface IEngineComponent
    {
        int MaxNumber { get; }
        bool IsFinished { get; }
        Board Panel { get; }
        void CreateEngine(EngineType type, int width, int height);
        Board GetNextIteration();
        void SetCellState(int x, int y, bool state);
        void ChangeCellState(int x, int y);
        void SetRule(int rule);
    }
}
