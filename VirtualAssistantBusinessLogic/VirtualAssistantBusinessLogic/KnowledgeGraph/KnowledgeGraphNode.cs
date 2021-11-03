using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAssistantBusinessLogic.KnowledgeGraph
{
    public class KnowledgeGraphNode
    {
        string Name { get; set; }
        Collection<string> Predicates { get; private set; }
    }
}
