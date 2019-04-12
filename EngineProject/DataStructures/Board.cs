using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineProject.DataStructures
{
    public class Board
    {
        public List<Cell> board { get; private set;}
        public Board() {
            board = new List<Cell>(new Cell[] { new Cell(), new Cell(true), new Cell(), new Cell(true), new Cell(true), new Cell(), new Cell(), new Cell(), new Cell() });

        }
    }
}
