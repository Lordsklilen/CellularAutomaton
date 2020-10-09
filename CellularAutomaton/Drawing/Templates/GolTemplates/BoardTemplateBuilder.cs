using EngineProject.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomaton.Drawing
{
    public class BoardTemplateBuilder
    {
        readonly Random rand = new Random();
        public void BuildClear(Board panel)
        {
            panel.Clear();
        }
        public void BuildRandom(Board panel)
        {
            panel.Clear();
            foreach (var row in panel.board)
            {
                foreach (var cell in row)
                {
                    if (rand.Next() % 2 == 0) {
                        cell.SetState(true);
                    }
                }
            }
        }
        public void BuildOscilator(Board panel)
        {
            panel.Clear();
            panel.SetCellState(2, 2, true);
            panel.SetCellState(2, 1, true);
            panel.SetCellState(2, 3, true);
        }
        public void BuildGlider(Board panel)
        {
            panel.Clear();
            panel.SetCellState(2, 2, true);
            panel.SetCellState(3, 3, true);
            panel.SetCellState(3, 4, true);
            panel.SetCellState(2, 4, true);
            panel.SetCellState(1, 4, true);
        }
    }
}
