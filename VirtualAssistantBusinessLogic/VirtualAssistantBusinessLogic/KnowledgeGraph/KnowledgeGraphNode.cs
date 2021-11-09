using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAssistantBusinessLogic.KnowledgeGraph
{
    public class KnowledgeGraphNode
    {
        public string Name { get; set; }
        public Dictionary<string, List<string>> Information { get; set; }
    }
}
