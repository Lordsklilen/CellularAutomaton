using EngineProject.DataStructures;
using System.Collections.Generic;
using System.Linq;

namespace EngineProject.Engines.NeighbourStrategy
{
    public interface INeighbourStrategy
    {
        Board CopyPanel { get; }
        void Initialize(Board panel, Board copyPanel, int _maxRow, int _maxColumn, bool OpenBorderCondition);
        void ComputeCell(Grain cell);
        List<int> NeighboursGrainNumbers(Grain cell);
    }
    
    public class Utils{
        public static int MostCommonNeighbour(List<int> neighbours)
        {
            if (neighbours.Count == 0)
                return 0;
            else
            {
                var groups = neighbours.GroupBy(x => x);
                return groups.OrderByDescending(x => x.Count()).First().Key;
            }
        }

    }
}
