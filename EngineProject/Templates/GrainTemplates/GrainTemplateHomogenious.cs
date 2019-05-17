using EngineProject.DataStructures;
using System;

namespace EngineProject.Templates.GrainTemplates
{
    public class GrainTemplateHomogenious : IGrainTemplateStrategy
    {
        public void GenerateTemplate(TemplateRequest request)
        {
            var numberOfPoints = request.numberOfPoints;
            var panel = request.board;

        }
    }
}
