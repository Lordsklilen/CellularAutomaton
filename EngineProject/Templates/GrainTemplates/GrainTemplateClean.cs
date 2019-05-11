using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineProject.Templates.GrainTemplates
{
    public class GrainTemplateClean : IGrainTemplateStrategy
    {
        public void GenerateTemplate(TemplateRequest request)
        {
            request.board.Clear();
        }
    }
}
