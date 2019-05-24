using EngineProject.Engines.NeighbourStrategy;

namespace EngineProject.Engines.MonteCarlo
{
    public class MonteCarloRequest
    {
        public int numberOfIterations;
        public double Kt;
        public bool borderConditions;
        public NeighbourStrategyRequest strategyRequest;
    }
}
