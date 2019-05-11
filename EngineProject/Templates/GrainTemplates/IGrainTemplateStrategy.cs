using EngineProject.DataStructures;

namespace EngineProject.Templates.GrainTemplates
{
    public interface IGrainTemplateStrategy
    {
        void GenerateTemplate(Board board, int parameter = -1);
    }
}
