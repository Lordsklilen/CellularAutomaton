using EngineProject.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EngineProject.Engines.NeighbourStrategy
{
    public static class NeighbourHelper
    {
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
