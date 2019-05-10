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
        public int x;
        public int y; 
        public bool state;
        private CellType type;

        public int X() => x;
        public int Y() => y;
        public bool GetState() => state;
        public CellType GetCellType() => type;
        public void SetState(bool _state) => state = _state;

        public Cell(int _x, int _y, CellType type = CellType.Cell, bool _state = false)
        {
            x = _x;
            y = _y;
            state = false;
            this.type = type;
        }

    }
}
