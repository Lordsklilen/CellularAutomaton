using EngineProject.DataStructures;

namespace EngineProject.Templates.GrainTemplates
{
    public interface IGrainTemplateStrategy
    {
        void GenerateTemplate(TemplateRequest request);
    }
}
