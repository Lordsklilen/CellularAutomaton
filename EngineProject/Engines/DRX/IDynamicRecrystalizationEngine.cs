﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineProject.Engines.DRX
{
    internal interface IDynamicRecrystalizationEngine
    {
        double CurrentTotalDensity { get; }
        double PreviousTotalDensity { get; }
    }
}
