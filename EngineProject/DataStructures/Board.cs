using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineProject.DataStructures
{
    public class Board
    {
        public Cell[][] board { get; private set;}
        private int width;
        private int height;
        
        public Board(int width, int height)
        {
            this.width = width;
            this.height = height;
            Clear();


        }
        public void SetCellState(int x, int y, bool state) {
            board[x][y].SetState(state);
        }
        public void Clear()
        {
            board = new Cell[height][];
            for (int i = 0; i < height; i++)
            {
                board[i] = new Cell[width];
                for (int j = 0; j < width; j++)
                {
                    board[i][j] = new Cell(i, j);
                }
            }
        }
    }
}
