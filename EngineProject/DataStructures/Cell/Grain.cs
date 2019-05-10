using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineProject.DataStructures
{
    public class Grain : Cell
    {
        private int grainNumber { get; set; }

        public Grain(int x, int y) : base(x, y) {
            grainNumber = 0;
        }
        public int GetGrainNumber() => grainNumber;
        public void SetGrainNumber(int number) => grainNumber = number;
    }
}
