
using EngineProject.DataStructures;
using EngineProject.Engines.NeighbourStrategy;

namespace EngineProject.Engines.MonteCarlo
{
    internal class MonteCarloEngine
    {
        private int iterations;
        private double Kt;
        private bool b;
        private NeighbourStrategyRequest strategyRequest;

        internal MonteCarloEngine(MonteCarloRequest request) {
            iterations = request.numberOfIterations;
            b = request.borderConditions;
            Kt = request.Kt;
            strategyRequest = request.strategyRequest;
        }
        internal int CalculateEnergy(Board board, int x, int y) {

            return 0;
        }
        internal Board ReCalculateAllEnergy(Board board)
        {
            return board;
        }

    }
}
