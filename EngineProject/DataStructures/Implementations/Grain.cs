using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineProject.DataStructures
{
    public class Grain :Cell
    {
        private int grainNumber { get; set; }

        public Grain(int x, int y) : base(x, y) { }
        public int GetGrainNumber() => grainNumber;
    }
}
