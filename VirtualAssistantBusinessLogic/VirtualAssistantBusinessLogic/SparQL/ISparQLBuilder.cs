using VirtualAssistantBusinessLogic.KnowledgeGraph;

namespace VirtualAssistantBusinessLogic.SparQL
{
    /// <summary>
    /// Interface for SparQL Builders
    /// </summary>
    public interface ISparQLBuilder
    {
        public string Query { get; set; }
        public string Build();
    }
}
