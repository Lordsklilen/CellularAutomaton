using EngineProject.DataStructures.interfaces;
using System.Collections.Generic;
using System.Drawing;

namespace EngineProject.DataStructures
{
    public class Board : IBoard
    {

        public ICell[][] board { get; private set; }
        public CellType cellType { get; set; }
        public NeighbooorhoodType neighbooorhoodType { get; set; }
        private ICellFactory cellFactory;
        private int width;
        private int height;
        private int maxGrainNumber;
        public int X => width;
        public int Y => height;
        public bool finished;

        public int MaxNumber() => maxGrainNumber;
        public int GetGrainNumber(int x, int y) => (board[x][y] as Grain).GetGrainNumber();

        public Board(Board main)
        {
            this.width = main.width;
            this.height = main.height;
            cellType = main.cellType;
            cellFactory = new CellFactory();
            maxGrainNumber = main.maxGrainNumber;
            Copy(main);
        }

        public Board(int width, int height, CellType type = CellType.Cell)
        {
            this.width = width;
            this.height = height;
            cellType = type;
            cellFactory = new CellFactory();
            maxGrainNumber = 0;
            Clear();
        }

        public void SetCellState(int x, int y, bool state)
        {
            board[x][y].SetState(state);
        }

        public void Clear()
        {
            board = new ICell[height][];
            for (int i = 0; i < height; i++)
            {
                board[i] = new ICell[width];
                for (int j = 0; j < width; j++)
                {
                    board[i][j] = cellFactory.CreateCell(cellType, i, j);
                }
            }
            maxGrainNumber = 0;
            finished = false;
        }

        public void Copy(Board main)
        {
            board = new ICell[height][];
            for (int i = 0; i < height; i++)
            {
                board[i] = new ICell[width];
                for (int j = 0; j < width; j++)
                {
                    board[i][j] = cellFactory.CreateCell(main.board[i][j] as Grain);
                }
            }
            maxGrainNumber = 0;
            finished = false;
        }

        public void SetGrainNumber(int number, int x, int y) {
            if (number > maxGrainNumber)
                maxGrainNumber = number;
            (board[x][y] as Grain).SetGrainNumber(number);
        }

        public List<Point> GetBorderGrainsCoordinates(){
            List<Point> result = new List<Point>();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var el = board[i][j] as Grain;
                    if (el.E > 0)
                        result.Add(new Point() { X = i, Y = j });
                }
            }
            return result;
        }

        public List<Point> GetNonBorderGrainsCoordinates()
        {
            List<Point> result = new List<Point>();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var el = board[i][j] as Grain;
                    if (el.E == 0)
                        result.Add(new Point() { X = i, Y = j });
                }
            }
            return result;
        }

        public double MinDensity()
        {
            double result = double.MaxValue;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var el = board[i][j] as Grain;
                    if (el.DyslocationDensity < result)
                        result = el.DyslocationDensity;
                }
            }
            return result;
        }

        public double MaxDensity()
        {
            double result = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var el = board[i][j] as Grain;
                    if (el.DyslocationDensity > result)
                        result = el.DyslocationDensity;
                }
            }
            return result;
        }
    }
}
