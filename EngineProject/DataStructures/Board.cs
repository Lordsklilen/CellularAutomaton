using EngineProject.DataStructures.interfaces;
using System.Collections.Generic;
using System.Drawing;

namespace EngineProject.DataStructures
{
    public class Board : IBoard
    {

        public ICell[][] BoardContainer { get; private set; }
        public CellType CellType { get; set; }
        public NeighborhoodType neighbooorhoodType { get; set; }
        private readonly ICellFactory cellFactory;
        private int maxGrainNumber;
        public int X { get; }
        public int Y { get; }
        public bool finished;
        public int maxRecrystalizedNumber;

        public int MaxNumber() => maxGrainNumber;
        public int GetGrainNumber(int x, int y) => (BoardContainer[x][y] as Grain).GetGrainNumber();

        public Board(Board main)
        {
            X = main.X;
            Y = main.Y;
            CellType = main.CellType;
            cellFactory = new CellFactory();
            maxGrainNumber = main.maxGrainNumber;
            Copy(main);
        }

        public Board(int width, int height, CellType type = CellType.Cell)
        {
            X = width;
            Y = height;
            CellType = type;
            cellFactory = new CellFactory();
            maxGrainNumber = 0;
            Clear();
        }

        public void SetCellState(int x, int y, bool state)
        {
            BoardContainer[x][y].SetState(state);
        }

        public void Clear()
        {
            BoardContainer = new ICell[Y][];
            for (int i = 0; i < Y; i++)
            {
                BoardContainer[i] = new ICell[X];
                for (int j = 0; j < X; j++)
                {
                    BoardContainer[i][j] = cellFactory.CreateCell(CellType, i, j);
                }
            }
            maxGrainNumber = 0;
            maxRecrystalizedNumber = 0;
            finished = false;
        }

        public void Copy(Board main)
        {
            BoardContainer = new ICell[Y][];
            for (int i = 0; i < Y; i++)
            {
                BoardContainer[i] = new ICell[X];
                for (int j = 0; j < X; j++)
                {
                    var el = main.BoardContainer[i][j] as Grain ?? main.BoardContainer[i][j] as Cell;
                    if(el.GetType() == typeof(Grain))
                        BoardContainer[i][j] = cellFactory.CreateGrain(el as Grain);
                    else 
                        BoardContainer[i][j] = cellFactory.CreateCell(el);
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
            (BoardContainer[x][y] as Grain).SetGrainNumber(number);
        }

        public void SetNewGrainNumber(int x, int y)
        {
            maxGrainNumber += 1;
            (BoardContainer[x][y] as Grain).SetGrainNumber(maxGrainNumber);
        }

        public void SetNewRecrystalizedNumber(int x, int y)
        {
            maxRecrystalizedNumber += 1;
            (BoardContainer[x][y] as Grain).RecrystalizedNumber = maxRecrystalizedNumber;
        }

        public List<Point> GetBorderGrainsCoordinates(){
            List<Point> result = new List<Point>();
            for (int i = 0; i < Y; i++)
            {
                for (int j = 0; j < X; j++)
                {
                    var el = BoardContainer[i][j] as Grain;
                    if (el.E > 0)
                        result.Add(new Point() { X = i, Y = j });
                }
            }
            return result;
        }

        public List<Point> GetNonBorderGrainsCoordinates()
        {
            List<Point> result = new List<Point>();
            for (int i = 0; i < Y; i++)
            {
                for (int j = 0; j < X; j++)
                {
                    var el = BoardContainer[i][j] as Grain;
                    if (el.E == 0)
                        result.Add(new Point() { X = i, Y = j });
                }
            }
            return result;
        }

        public decimal MinDensity()
        {
            decimal result = decimal.MaxValue;
            for (int i = 0; i < Y; i++)
            {
                for (int j = 0; j < X; j++)
                {
                    var el = BoardContainer[i][j] as Grain;
                    if (el.DyslocationDensity < result)
                        result = el.DyslocationDensity;
                }
            }
            return result;
        }

        public decimal MaxDensity()
        {
            decimal result = 0;
            for (int i = 0; i < Y; i++)
            {
                for (int j = 0; j < X; j++)
                {
                    var el = BoardContainer[i][j] as Grain;
                    if (el.DyslocationDensity > result)
                        result = el.DyslocationDensity;
                }
            }
            return result;
        }
    }
}
