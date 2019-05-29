﻿using EngineProject.DataStructures.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineProject.DataStructures
{
    public interface ICellFactory
    {
        ICell CreateCell(CellType type, int x, int y);
        Grain CreateCell(Grain grain);
    }
}
