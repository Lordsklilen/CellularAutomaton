using EngineProject.DataStructures;
using EngineProject.Engines.NeighbourStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineProject.Engines.DRX
{
    internal interface IDynamicRecrystalizationEngine
    {
        IList<DensitySnapshot> TotalDensityList { get; }

        void Initialize(DRXRequest r, INeighbourStrategy strategy);
        Board IterateAll(Board board);
        Board NextIteration(Board board, decimal t);
        string GetSaveText();
    }
}
