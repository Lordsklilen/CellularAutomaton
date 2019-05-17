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
        private NeighbooorhoodType neighbooorhoodType { get; set; }
        public Grain(int x, int y, NeighbooorhoodType nType = NeighbooorhoodType.VonNeumann) : base(x, y) {
            grainNumber = 0;
            type = CellType.Grain;
            neighbooorhoodType = nType;
        }
        public int GetGrainNumber() => grainNumber;
        public void SetGrainNumber(int number) => grainNumber = number;
    }
}
