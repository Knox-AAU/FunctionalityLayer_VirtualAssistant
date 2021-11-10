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
            return new EncodedSPO($"?s{id} ?p \"{subject}\"@en . ", $"?s{id++} ");
        }
        public EncodedSPO EncodePredicate(string predicate)
        {
            switch (predicate.ToLower())
            {
                case "spouse": return new EncodedSPO("", "wdt:P26");
                case "birth name": return new EncodedSPO("", "wdt:P1477");
                case "date of birth": return new EncodedSPO("", "wdt:P569");
                case "occupation": return new EncodedSPO("", "wdt:P106");
                default: return new EncodedSPO("", "wdt:P31");
            }
        }
        public EncodedSPO EncodeObject(string obj)
        {
            throw new NotImplementedException();
        }
    }
}
