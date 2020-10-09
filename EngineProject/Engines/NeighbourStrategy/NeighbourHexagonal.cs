using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineProject.DataStructures;

namespace EngineProject.Engines.NeighbourStrategy
{
    public class NeighbourHexagonal : INeighbourStrategy
    {
        private Board panel;
        private int _maxRow;
        private int _maxColumn;
        private bool OpenBorderCondition;
        public HexType type { get; set; } = HexType.Left;

        readonly Random rand = new Random();

        public Board CopyPanel { get; private set; }
        public int N => 6;
        public void Initialize(Board panel, Board copyPanel, int _maxRow, int _maxColumn, bool OpenBorderCondition)
        {
            this.panel = panel;
            CopyPanel = copyPanel;
            this._maxRow = _maxRow;
            this._maxColumn = _maxColumn;
            this.OpenBorderCondition = OpenBorderCondition;
        }

        public void ComputeCell(Grain cell)
        {
            if (cell.GetGrainNumber() == 0)
            {
                var neighbours = NeighboursGrainNumbers(cell);
                CopyPanel.SetGrainNumber(NeighbourHelper.MostCommonNeighbour(neighbours), cell.x, cell.y);
                CopyPanel.finished = false;
            }
            else
                CopyPanel.SetGrainNumber(cell.GetGrainNumber(), cell.x, cell.y);
        }

        public List<int> NeighboursGrainNumbers(Grain cell)
        {
            return GetOnlyGrainNumbers(NeighboursGrainCells(cell));
        }

        public List<int> GetOnlyGrainNumbers(List<Grain> cells)
        {
            return cells.Select(x => x.GetGrainNumber()).ToList();
        }

        public List<int> GetOnlyRecrystalizationNumbers(List<Grain> cells)
        {
            return cells.Select(x => x.RecrystalizedNumber).ToList();
        }

        public int GetRecrystalizedAndGrainGrains(List<Grain> grains, int recrystalizationNumber, int grainNumber) {

            return grains.Count(x => (x.GetGrainNumber() != 0 && x.GetGrainNumber() != grainNumber) ||
                (x.RecrystalizedNumber != 0 && x.RecrystalizedNumber != recrystalizationNumber));
        }

        private int[,] GeneratePairs(HexType type)
        {
            if (type == HexType.Left)
                return new int[,]{
                    { 0,1  },
                    { 1,1  },
                    { 1,0  },
                    { 0,-1 },
                    { -1,-1 },
                    { -1,0  } };//left
            if (type == HexType.Right)
                return new int[,]{
                    { -1,0 },
                    { -1,1 },
                    { 0,1  },
                    { 0,-1 },
                    { 1,-1 },
                    { 1,0  } };//right
            throw new Exception("This direction is not supprted");
        }

        public List<Grain> NeighboursGrainCells(Grain cell)
        {
            List<Grain> neighbours = new List<Grain>();
            int[,] pairs;
            HexType currentType;
            if (type == HexType.Random)
            {
                int dir = rand.Next(0, 2);
                if (dir == 0)
                    currentType = HexType.Left;
                else
                    currentType = HexType.Right;
            }
            else
                currentType = type;
            pairs = GeneratePairs(currentType);
            for (int i = 0; i < 6; i++)
            {
                int x = pairs[i, 0];
                int y = pairs[i, 1];
                int widthId;
                int heightId;
                int number;
                if (OpenBorderCondition)
                {
                    widthId = (x + cell.x) >= 0 ? (x + cell.x) % (_maxRow) : _maxRow - 1;
                    heightId = (y + cell.y) >= 0 ? (y + cell.y) % (_maxColumn) : _maxColumn - 1;
                    number = ((Grain)panel.BoardContainer[widthId][heightId]).GetGrainNumber();
                }
                else
                {
                    widthId = (x + cell.x);
                    heightId = (y + cell.y);

                    if (widthId < 0 || heightId < 0 || widthId >= _maxRow || heightId >= _maxColumn)
                        number = 0;
                    else
                        number = ((Grain)panel.BoardContainer[widthId][heightId]).GetGrainNumber();
                }

                if (number > 0)
                    neighbours.Add((Grain)panel.BoardContainer[widthId][heightId]);
            }
            return neighbours;
        }
    }
}
