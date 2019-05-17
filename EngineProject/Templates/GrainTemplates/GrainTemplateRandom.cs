using EngineProject.DataStructures;
using System;

namespace EngineProject.Templates.GrainTemplates
{
    public class GrainTemplateRandom : IGrainTemplateStrategy
    {
        Random rand = new Random();
        public void GenerateTemplate(TemplateRequest request)
        {
            var numberOfPoints = request.numberOfPoints;
            var panel = request.board;
            if (numberOfPoints < 1)
                throw new NotImplementedException("Cannot generate template with number of points: " + numberOfPoints.ToString());

            panel.Clear();
            int NumberOfWrongShots = 0;
            for (int i = 0; i < numberOfPoints; i++)
            {
                if (NumberOfWrongShots > 100000)
                    throw new ArgumentOutOfRangeException("Cannot add that many points. Propably number of expected points is to big.");
                int row = rand.Next(0, panel.Y);
                int collumn = rand.Next(0, panel.X);
                if (panel.GetGrainNumber(row,collumn) == 0)
                {
                    panel.SetGrainNumber(i,row, collumn);
                }
                else {
                    --i;
                    ++NumberOfWrongShots;
                }
            }
        }
    }
}
