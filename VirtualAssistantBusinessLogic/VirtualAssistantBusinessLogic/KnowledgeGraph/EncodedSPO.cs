using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAssistantBusinessLogic.KnowledgeGraph
{
    public class EncodedSPO
    {
        public EncodedSPO(string triplet, string name)
        {
            Triplet = triplet;
            Name = name;
        }
        public string Triplet { get; set; }
        public string Name { get; set; }
    }
}
