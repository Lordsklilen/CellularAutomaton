using EngineProject.DataStructures;

namespace EngineProject.Engines
{
    public interface IEngine
    {
        Board Panel { get; }
        Board NextIteration();
        void SetCellState(int x, int y, bool state);
        void ChangeCellState(int x, int y);
    }
}
