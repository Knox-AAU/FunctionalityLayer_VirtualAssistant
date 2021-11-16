using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAssistantBusinessLogic.KnowledgeGraph
{
    /// <summary>
    /// Encoder to encode subject, predicate and objects to the wikidata ontology.
    /// </summary>
    public class WikidataSPOEncoder : ISPOEncoder
    {
        private int id = 0;
        /// <summary>
        /// Encodes the subject returning a encoded spo with a triplet to get
        /// the nodes with a label equal to the subject and a unique name for the
        /// triplet results.
        /// </summary>
        /// <param name="subject">Name/Label for the subject to encode</param>
        /// <returns>EncodedSPO for the subject</returns>
        public EncodedSPO EncodeSubject(string subject)
        {
            return new EncodedSPO($"?s{id} ?p \"{subject}\"@en . ", $"?s{id++} ");
        }

        /// <summary>
        /// Encodes the predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Not used so not implemented - Will throw a NotImplementedException!
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public EncodedSPO EncodeObject(string obj)
        {
            throw new NotImplementedException();
        }
    }
}
