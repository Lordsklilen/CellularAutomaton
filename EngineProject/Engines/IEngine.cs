using EngineProject.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineProject.Engines
{
    public interface IEngine
    {
        Board panel { get; }
        void NextIteration();
        Board GetBoard();
        void SetRule(int rule);
        void SetCellState(int x, int y, bool state);
        void SetGrainNumber(int number, int x, int y);
        void ChangeCellState(int x, int y);
        void ChangeBorderConditions(bool state);

    }
}
