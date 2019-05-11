using EngineProject.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineProject
{
    public interface IEngineComponent
    {
        void CreateEngine(EngineType type,int width, int height);
        Board GetNextIteration();
        Board GetBoard();
        void SetCellState(int x, int y, bool state);
        void ChangeCellState(int x, int y);
        void SetRule(int rule);
        void SetGrainNumber(int grainNumber, int x, int y);
    }
}
