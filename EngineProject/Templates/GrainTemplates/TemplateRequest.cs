using EngineProject.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineProject.Templates.GrainTemplates
{
    public class TemplateRequest
    {
        public int radius;
        public int numberOfPoints;
        public GrainTemplateType type;
        public Board board;
        public int x;
        public int y; 
    }
}
