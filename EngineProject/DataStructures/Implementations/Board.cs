using EngineProject.DataStructures.interfaces;


namespace EngineProject.DataStructures
{
    public class Board:IBoard
    {

        public ICell[][] board { get; private set;}
        public CellType cellType { get; set; }
        private ICellFactory cellFactory;
        private int width;
        private int height;

        public Board(int width, int height, CellType type = CellType.Cell)
        {
            this.width = width;
            this.height = height;
            cellType = type;
            cellFactory = new CellFactory();
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
        }

    }
}
