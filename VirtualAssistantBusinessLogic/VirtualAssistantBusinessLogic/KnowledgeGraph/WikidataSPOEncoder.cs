using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAssistantBusinessLogic.KnowledgeGraph
{
    public class WikidataSPOEncoder : ISPOEncoder
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
                case "continent": return new EncodedSPO("", "p:P30/ps:P30");
                case "official language": return new EncodedSPO("", "p:P37/ps:P37");
                case "capital": return new EncodedSPO("", "p:P36/ps:P36");
                case "population": return new EncodedSPO("", "p:P1082/ps:P1082");
                default: return new EncodedSPO("", "wdt:P31");
            }
        }
        public EncodedSPO EncodeObject(string obj)
        {
            throw new NotImplementedException();
        }
    }
}
