using EngineProject.DataStructures.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineProject.DataStructures
{
    public class Cell : ICell
    {
        public bool state { get; set; }
        public int x { get; private set; }
        public int y { get; private set; }
        public int X() => x;
        public int Y() => y;
        public bool GetState() => state;

        public Cell(bool _state = false) {
            state = _state;
        }

        public Cell(int _x, int _y, bool _state = false)
        {
            x = _x;
            y = _y;
            state = false;
        }

        public void SetState(bool _state)
        {
            state = _state;
        }
    }
}
