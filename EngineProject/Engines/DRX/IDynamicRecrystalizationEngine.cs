using EngineProject.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineProject.Engines.DRX
{
    internal interface IDynamicRecrystalizationEngine
    {
        //    double TotalRo { get; }
        //    double PreviousTotalRo { get; }
        IList<DensitySnapshot> TotalDensityList { get; }

        void Initialize(DRXRequest r);
        Board IterateAll(Board board);
        Board NextIteration(Board board, double t);
        string GetSaveText();
    }
}
