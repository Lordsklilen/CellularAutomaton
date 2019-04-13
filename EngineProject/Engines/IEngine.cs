using EngineProject.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineProject.Engines
{
    public interface IEngine
    {
        void NextIteration();
        Board GetBoard();
        void SetCellState(int x, int y,bool state);
    }
}
