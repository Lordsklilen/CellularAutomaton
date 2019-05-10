using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineProject.DataStructures.interfaces
{
    public interface IBoard
    {
        void SetCellState(int x, int y, bool state);
        void Clear();
    }
}
