﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineProject.DataStructures
{
    public class Grain : Cell
    {
        private int grainNumber { get; set; }
        public double CenterOfX { get; set; }
        public double CenterOfY { get; set; }
        public int E { get; set; } = 0;

        public Grain(int x, int y) : base(x, y)
        {
            grainNumber = 0;
            type = CellType.Grain;
        }

        public int GetGrainNumber() => grainNumber;
        public void SetGrainNumber(int number) => grainNumber = number;
        public Point GetMassCenter() => new Point(CenterOfX + x, CenterOfY + y);

    }
}
