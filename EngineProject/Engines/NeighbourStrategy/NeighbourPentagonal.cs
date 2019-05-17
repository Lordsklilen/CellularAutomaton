using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineProject.DataStructures;

namespace EngineProject.Engines.NeighbourStrategy
{
    public class NeighbourPentagonal : INeighbourStrategy
    {
        private Board panel;
        private Board copyPanel;
        private int _maxRow;
        private int _maxColumn;
        private bool OpenBorderCondition;
        Random rand = new Random();

        public Board CopyPanel => copyPanel;

        public void Initialize(Board panel, Board copyPanel, int _maxRow, int _maxColumn, bool OpenBorderCondition)
        {
            this.panel = panel;
            this.copyPanel = copyPanel;
            this._maxRow = _maxRow;
            this._maxColumn = _maxColumn;
            this.OpenBorderCondition = OpenBorderCondition;
        }

        public void ComputeCell(Grain cell)
        {
            if (cell.GetGrainNumber() == 0)
            {
                int nextGrainNumber = MostCommonNeighbour(cell as Grain);
                copyPanel.SetGrainNumber(nextGrainNumber, cell.x, cell.y);
                copyPanel.finished = false;
            }
            else
                copyPanel.SetGrainNumber(cell.GetGrainNumber(), cell.x, cell.y);
        }

        private int MostCommonNeighbour(Grain cell)
        {
            List<int> neighbours = new List<int>();
            int[,] pairs = GeneratePairs();
            for (int i = 0; i <= 4; i++)
            {
                int x = pairs[i, 0];
                int y = pairs[i, 1];
                int widthId = 0;
                int heightId = 0;
                int number = 0;
                if (OpenBorderCondition)
                {
                    widthId = (x + cell.x) >= 0 ? (x + cell.x) % (_maxRow) : _maxRow - 1;
                    heightId = (y + cell.y) >= 0 ? (y + cell.y) % (_maxColumn) : _maxColumn - 1;
                    number = ((Grain)panel.board[widthId][heightId]).GetGrainNumber();
                }
                else
                {
                    widthId = (x + cell.x);
                    heightId = (y + cell.y);

                    if (widthId < 0 || heightId < 0 || widthId >= _maxRow || heightId >= _maxColumn)
                        number = 0;
                    else
                        number = ((Grain)panel.board[widthId][heightId]).GetGrainNumber();
                }

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
        public int[,] GeneratePairs()
        {
            int dir = rand.Next(0, 4);
            if (dir == 0)
                return new int[,]{
                    { 0, 1 },
                    { -1, 1 },
                    { -1, 0 },
                    { -1, -1 },
                    { 0, -1 } };//left
            if (dir == 1)
                return new int[,]{
                    { 0, 1 },
                    { 1, 1 },
                    { 1, 0 },
                    { 1, -1 },
                    { 0, -1 } };//right
            if (dir == 2)
                return new int[,]{
                    { -1, 0 },
                    { -1, 1 },
                    { 0, 1 },
                    { 1, 1 },
                    { 1, 0 } };//top
            if (dir == 3)
                return new int[,]{
                    { -1, 0},
                    { -1, -1 },
                    { 0, -1 },
                    { 1, -1 },
                    { 1, 0 } };//bottom
            throw new Exception("This direction is not supprted");
        }
    }
}
