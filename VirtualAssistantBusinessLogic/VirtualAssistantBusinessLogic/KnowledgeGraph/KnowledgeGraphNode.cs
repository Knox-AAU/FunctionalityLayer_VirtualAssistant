using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAssistantBusinessLogic.KnowledgeGraph
{
    /// <summary>
    /// Knowledge Graph Node containing the Id of the node, 
    /// Name (label) and Information for the node.
    /// </summary>
    public class KnowledgeGraphNode
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, List<string>> Information { get; set; }
    }
}
