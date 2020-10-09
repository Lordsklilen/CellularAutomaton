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
        private readonly ICellFactory cellFactory;
        private readonly int width;
        private readonly int height;
        private int maxGrainNumber;
        public int X => width;
        public int Y => height;
        public bool finished;
        public int maxRecrystalizedNumber;

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
            maxRecrystalizedNumber = 0;
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
                    var el = main.board[i][j] as Grain ?? main.board[i][j] as Cell;
                    if(el.GetType() == typeof(Grain))
                        board[i][j] = cellFactory.CreateGrain(el as Grain);
                    else 
                        board[i][j] = cellFactory.CreateCell(el);
                }
            }
            maxGrainNumber = main.maxGrainNumber;
            maxRecrystalizedNumber = main.maxRecrystalizedNumber;
            finished = false;
        }

        public void SetGrainNumber(int number, int x, int y)
        {
            if (number > maxGrainNumber)
                maxGrainNumber = number;
            (board[x][y] as Grain).SetGrainNumber(number);
        }

        public void SetNewGrainNumber(int x, int y)
        {
            maxGrainNumber += 1;
            (board[x][y] as Grain).SetGrainNumber(maxGrainNumber);
        }

        public void SetNewRecrystalizedNumber(int x, int y)
        {
            maxRecrystalizedNumber += 1;
            (board[x][y] as Grain).RecrystalizedNumber = maxRecrystalizedNumber;
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

        public decimal MinDensity()
        {
            decimal result = decimal.MaxValue;
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

        public decimal MaxDensity()
        {
            decimal result = 0;
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
