using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAssistantBusinessLogic.KnowledgeGraph
{
    public class SPOEncoder
    {
        private int id = 0;
        public EncodedSPO EncodeSubject(string subject)
        {
            return new EncodedSPO($"?s{id} rdfs:label \"{subject}\"@en . ", $"?s{id++} ");
        }
        public EncodedSPO EncodePredicate(string predicate)
        {
            switch (predicate.ToLower())
            {
                case "spouse": return new EncodedSPO("", "dbo:spouse");
                case "birth name": return new EncodedSPO("", "dbo:birthName");
                case "date of birth": return new EncodedSPO("", "dbo:birthDate");
                case "occupation": return new EncodedSPO("", "dbo:occupation");
                case "type": return new EncodedSPO("", "rdf:type");
                default: throw new ArgumentException($"Unknown predicate: '{predicate}'");
            }
        }
        public EncodedSPO EncodeObject(string obj)
        {
            throw new NotImplementedException();
        }
    }
}
