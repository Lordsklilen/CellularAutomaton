using EngineProject.DataStructures;

namespace EngineProject.Engines.NeighbourStrategy
{
    public interface INeighbourStrategy
    {
        void ComputeCell(Grain cell, Board copyPanel);
    }
}
