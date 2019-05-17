using EngineProject.DataStructures;
using EngineProject.Templates.GrainTemplates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineProject
{
    public interface IEngineComponent
    {
        int MaxNumber { get; }
        bool IsFinished { get; }
        Board Board { get; }
        void CreateEngine(EngineType type,int width, int height);
        Board GetNextIteration();
        void SetCellState(int x, int y, bool state);
        void ChangeCellState(int x, int y);
        void SetRule(int rule);
        void SetGrainNumber(int grainNumber, int x, int y);
        void GenerateGrainTemplate(TemplateRequest request);
        void ChangeBorderConditions(bool state);
        void ChangeNeighbooroodType(NeighbooorhoodType type);
    }
}
