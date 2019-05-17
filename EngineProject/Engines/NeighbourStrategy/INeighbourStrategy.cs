using EngineProject.DataStructures;

namespace EngineProject.Engines.NeighbourStrategy
{
    public interface INeighbourStrategy
    {
        Board CopyPanel { get; }
        void Initialize(Board panel, Board copyPanel, int _maxRow, int _maxColumn, bool OpenBorderCondition);
        void ComputeCell(Grain cell);
    }
}
