using EngineProject.DataStructures;

namespace EngineProject.Engines
{
    public interface IEngine
    {
        Board Panel { get; }
        void NextIteration();
        void SetCellState(int x, int y, bool state);
        void SetGrainNumber(int number, int x, int y);
        void ChangeCellState(int x, int y);
        void ChangeBorderConditions(bool state);
    }
}
