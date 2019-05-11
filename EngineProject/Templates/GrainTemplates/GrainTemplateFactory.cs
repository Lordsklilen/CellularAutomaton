using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineProject.Templates.GrainTemplates
{
    public class GrainTemplateFactory
    {
        public IGrainTemplateStrategy CreateTemplate(GrainTemplateType type) {
            switch (type)
            {
                case GrainTemplateType.Clear:
                case GrainTemplateType.Random:
                    return new GrainTemplateRandom();
                case GrainTemplateType.Radius:
                default:
                    throw new NotSupportedException("this type of template is not supproted");
            }
        }
    }
}
