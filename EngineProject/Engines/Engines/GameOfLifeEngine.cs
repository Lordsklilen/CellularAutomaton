using EngineProject.DataStructures;

namespace EngineProject.Engines
{
    public class GameOfLifeEngine : IEngine
    {
        public Board Panel { get; private set; }
        public EngineType type;
        private readonly int _maxRow;
        private readonly int _maxColumn;

        public GameOfLifeEngine(int width, int height)
        {
            Panel = new Board(width, height);
            type = EngineType.GameOfLife;
            _maxRow = height;
            _maxColumn = width;
        }

        public Board NextIteration()
        {
            var copyPanel = new Board(Panel);
            foreach (var row in Panel.BoardContainer)
            {
                foreach (var cell in row)
                {
                    ComputeCell(cell as Cell, copyPanel);
                }

            }
            Panel = copyPanel;
            return Panel;
        }

        private void ComputeCell(Cell cell, Board copyPanel)
        {
            int neighbours = CheckNeighbours(cell);
            if (cell.state)
            {
                if (neighbours == 2 || neighbours == 3)
                    copyPanel.BoardContainer[cell.x][cell.y].SetState(true);
                else
                    copyPanel.BoardContainer[cell.x][cell.y].SetState(false);
            }
            else
            {
                if (neighbours == 3)
                    copyPanel.BoardContainer[cell.x][cell.y].SetState(true);
                else
                    copyPanel.BoardContainer[cell.x][cell.y].SetState(false);
            }
        }
        public void ChangeCellState(int x, int y)
        {
            Panel.SetCellState(x, y, !Panel.BoardContainer[x][y].GetState());
        }

        public void SetCellState(int x, int y, bool state)
        {
            Panel.SetCellState(x, y, state);
        }

        private int CheckNeighbours(Cell cell)
        {
            int counter = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int widthId = (i + cell.x) >= 0 ? (i + cell.x) % (_maxRow) : _maxRow - 1;
                    int heightId = (j + cell.y) >= 0 ? (j + cell.y) % (_maxColumn) : _maxColumn - 1;

                    if (Panel.BoardContainer[widthId][heightId].GetState() && !(i == 0 && j == 0))
                        counter++;
                }
            }
            return counter;
        }
    }
}
