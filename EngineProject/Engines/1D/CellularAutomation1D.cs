using EngineProject.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineProject.Engines
{
    public class CellularAutomation1D : IEngine
    {
        public Board board { get; private set; }

        public CellularAutomation1D() {
            board = new Board();
        }

        public Board GetBoard()
        {
            return board;
        }

        public void NextIteration()
        {

        }
    }
}
