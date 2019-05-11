using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineProject.DataStructures;
using EngineProject.DataStructures.interfaces;

namespace EngineProject.Engines.Engines
{
    public class GrainGrowthEngine : IEngine
    {
        public Board panel { get; private set; }
        public EngineType type;
        private CellType cellType;
        private int _maxRow;
        private int _maxColumn;
        private bool finished;

        public bool IsFinished() => finished;        
        public Board GetBoard() => panel;

        public GrainGrowthEngine(int width, int height)
        {
            type = EngineType.GrainGrowth;
            cellType = CellType.Grain;
            panel = new Board(width, height, cellType);
            _maxRow = height;
            _maxColumn = width;
        }

        public void NextIteration()
        {
            finished = true;
            var copyPanel = new Board(_maxColumn, _maxRow, cellType);
            foreach (var row in panel.board)
            {
                foreach (var cell in row)
                {
                    ComputeCell((Grain)cell, copyPanel);
                }
            }
            panel = copyPanel;
        }

        private void ComputeCell(Grain cell, Board copyPanel)
        {
            if (cell.GetGrainNumber() == 0)
            {
                int nextGrainNumber = MostCommonNeighbour(cell as Grain);
                copyPanel.SetGrainNumber(nextGrainNumber, cell.x, cell.y);
                finished = false;
            }
            else
                copyPanel.SetGrainNumber(cell.GetGrainNumber(), cell.x, cell.y);
        }
        public void ChangeCellState(int x, int y)
        {
            panel.SetCellState(x, y, !panel.board[x][y].GetState());
        }

        public void SetCellState(int x, int y, bool state)
        {
            panel.SetCellState(x, y, state);
        }

        private int MostCommonNeighbour(Grain cell)
        {
            List<int> neighbours = new List<int>();
            int[,] pairs = { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };

            for (int i = 0; i <= 3; i++)
            {
                int x = pairs[i, 0];
                int y = pairs[i, 1];
                int widthId = (x + cell.x) >= 0 ? (x + cell.x) % (_maxRow) : _maxRow - 1;
                int heightId = (y + cell.y) >= 0 ? (y + cell.y) % (_maxColumn) : _maxColumn - 1;
                int number = ((Grain)panel.board[widthId][heightId]).GetGrainNumber();
                if (number > 0)
                    neighbours.Add(number);
            }
            if (neighbours.Count == 0)
                return 0;
            else
            {
                var groups = neighbours.GroupBy(x => x);
                return groups.OrderByDescending(x => x.Count()).First().Key;
            }
        }
        public void SetRule(int rule)
        {
            throw new NotImplementedException();
        }

        public void SetGrainNumber(int number, int x, int y)
        {
            panel.SetGrainNumber(number, x, y);
        }
    }
}
