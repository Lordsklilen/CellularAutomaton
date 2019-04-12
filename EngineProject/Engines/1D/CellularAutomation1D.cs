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

        public CellularAutomation1D(int width, int height) {
            board = new Board(width, height);
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
