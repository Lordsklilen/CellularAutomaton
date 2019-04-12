using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineProject.DataStructures
{
    public class Cell
    {
        public bool state { get; set; }

        public Cell() {
            state = false;
        }

        public Cell(bool _state) {
            state = _state;
        }
    }
}
