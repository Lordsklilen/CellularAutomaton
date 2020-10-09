using EngineProject.DataStructures;
using System;

namespace EngineProject.Templates.GrainTemplates
{
    public class GrainTemplateHomogenious : IGrainTemplateStrategy
    {
        public void GenerateTemplate(TemplateRequest request)
        {
            var numberX = request.x;
            var numberY = request.y;
            var panel = request.board;
            double dx = panel.X / ((double)numberX);
            double dy = panel.Y / ((double)numberY);
            int sx = (int)(dx / 2.0);
            int sy = (int)(dy / 2.0);

            if (dx < 1.0 || dy < 1.0 || request.x > panel.X || request.y > panel.Y)
                throw new Exception("Bad request for doing homogenious request");
            panel.Clear();

            var numberOfPoints = 1;
            for (int i = 0; i < numberX; i++)
            {
                for (int j = 0; j < numberY; j++)
                {
                    panel.SetGrainNumber(numberOfPoints++,(int)(j * dy)+sy,(int)(i * dx)+sx);
                }
            }
        }
    }
}
