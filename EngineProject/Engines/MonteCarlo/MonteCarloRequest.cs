using EngineProject.DataStructures;
using EngineProject.Engines.NeighbourStrategy;

namespace EngineProject.Engines.MonteCarlo
{
    public class MonteCarloRequest
    {
        public int numberOfIterations;
        public double Kt;
        public NeighbourStrategyRequest strategyRequest;
        public bool border;
        internal Board board;
        internal Board CopyBoard;
        internal int maxColumn;
        internal int maxRow;
    }
}
