using VirtualAssistantBusinessLogic.KnowledgeGraph;

namespace VirtualAssistantBusinessLogic.SparQL
{
    public interface ISparQLBuilder
    {
        public string Query { get; set; }
        public string Build();
    }
}
