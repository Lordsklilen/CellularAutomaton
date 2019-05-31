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
        private int maxRow;
        private int maxColumn;
        private bool OpenBorderCondition;
        public double radius;

        public Board CopyPanel => copyPanel;
        public int N => (int)(radius * radius) + 1;

        public void Initialize(Board panel, Board copyPanel, int _maxRow, int _maxColumn, bool OpenBorderCondition)
        {
            this.panel = panel;
            this.copyPanel = copyPanel;
            this.maxRow = _maxRow;
            this.maxColumn = _maxColumn;
            this.OpenBorderCondition = OpenBorderCondition;
        }

        public void ComputeCell(Grain cell)
        {
            if (cell.GetGrainNumber() == 0)
            {
                var neighbours = NeighboursGrainNumbers(cell as Grain);
                copyPanel.SetGrainNumber(NeighbourHelper.MostCommonNeighbour(neighbours), cell.x, cell.y);
                copyPanel.finished = false;
            }
            else
                copyPanel.SetGrainNumber(cell.GetGrainNumber(), cell.x, cell.y);
        }

        public List<int> NeighboursGrainNumbers(Grain cell)
        {
            List<int> neighbours = new List<int>();
            int RadiusTop = (int)radius + 1;
            Point centerOfMass = cell.GetMassCenter();
            for (int i = -RadiusTop; i <= RadiusTop; i++)
            {
                for (int j = -RadiusTop; j <= RadiusTop; j++)
                {
                    if (i == 0 && j == 0)
                        continue;

                    int number = 0;
                    int widthId;
                    int heightId;
                    if (OpenBorderCondition)
                    {
                        widthId = (i + cell.x) >= 0 ? (i + cell.x) % (maxRow) : (i + cell.x) + maxRow;
                        heightId = (j + cell.y) >= 0 ? (j + cell.y) % (maxColumn) : (j + cell.y) + maxColumn;
                    }
                    else
                    {
                        widthId = (i + cell.x);
                        heightId = (j + cell.y);
                        if (widthId < 0 || heightId < 0 || widthId >= maxRow || heightId >= maxColumn)
                            continue;
                    }

                    Grain colleague = (Grain)panel.board[widthId][heightId];
                    number = colleague.GetGrainNumber();
                    if (number == 0)
                        continue;
                    Point NeighbourMassCenter = colleague.GetInsideMassCenter();
                    NeighbourMassCenter.X += (j + cell.y);
                    NeighbourMassCenter.Y += (i + cell.x);

                    //Point NeighbourMassCenter = colleague.GetMassCenter();

                    double r = Math.Sqrt((Math.Pow(NeighbourMassCenter.X - centerOfMass.X, 2) + Math.Pow(NeighbourMassCenter.Y - centerOfMass.Y, 2)));
                    if (r <= radius)
                    {
                        number = colleague.GetGrainNumber();
                        if (number > 0)
                            neighbours.Add(number);
                    }
                }
            }
            return neighbours;
        }
    }
}
