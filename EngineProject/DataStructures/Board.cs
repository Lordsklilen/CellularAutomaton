using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineProject.DataStructures
{
    public class Board
    {
        public List<List<Cell>> board { get; private set;}
        public Board(int width, int height)
        {
            board = new List<List<Cell>>();
            for (int i = 0; i < height; i++)
            {
                board.Add(new List<Cell>());
                for (int j = 0; j < width; j++)
                {
                    board[i].Add(new Cell(i, j));
                }
            }
        }
    }
}
