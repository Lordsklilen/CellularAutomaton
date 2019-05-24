using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using EngineProject.DataStructures;

namespace EngineProject.Engines.NeighbourStrategy
{
    public class NeighbourRadius : INeighbourStrategy
    {
        private Board panel;
        private Board copyPanel;
        private int _maxRow;
        private int _maxColumn;
        private bool OpenBorderCondition;
        public double radius;

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
            int RadiusTop = (int)radius + 1;
            Point centerOfMass = cell.GetMassCenter();
            double X = centerOfMass.X;
            double Y = centerOfMass.Y;
            for (int i = -RadiusTop; i <= RadiusTop; i++)
            {
                for (int j = -RadiusTop; j <= RadiusTop; j++)
                {
                    int number = 0;
                    int widthId;
                    int heightId;
                    if (OpenBorderCondition)
                    {
                        widthId = (i + cell.x) >= 0 ? (i + cell.x) % (_maxRow) : _maxRow - 1;
                        heightId = (j + cell.y) >= 0 ? (j + cell.y) % (_maxColumn) : _maxColumn - 1;
                    }
                    else
                    {
                        widthId = (i + cell.x);
                        heightId = (j + cell.y);
                        if (widthId < 0 || heightId < 0 || widthId >= _maxRow || heightId >= _maxColumn)
                            number = 0;
                        else
                            number = ((Grain)panel.board[widthId][heightId]).GetGrainNumber();
                    }

                    Grain colleague = (Grain)panel.board[widthId][heightId];
                    Point NeighbourMassCenter = colleague.GetMassCenter();
                    if (Math.Sqrt((Math.Pow(NeighbourMassCenter.X - X, 2) + Math.Pow(NeighbourMassCenter.Y - Y, 2))) <= radius)
                        number = colleague.GetGrainNumber();
                    if (number > 0)
                        neighbours.Add(number);
                }
            }
            if (neighbours.Count == 0)
                return 0;
            else
            {
                var groups = neighbours.GroupBy(x => x);
                return groups.OrderByDescending(x => x.Count()).First().Key;
            }
        }
    }
}
