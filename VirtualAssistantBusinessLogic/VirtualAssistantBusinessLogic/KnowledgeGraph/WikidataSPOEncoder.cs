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
        /// Encodes the subject, returning an encoded spo. The encoded spo has a triplet to get
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
        /// Encodes the predicate by returning the wikidata id for the given predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public EncodedSPO EncodePredicate(string predicate)
        {
            return predicate.ToLower() switch
            {
                "spouse" => new EncodedSPO("", "wdt:P26"),
                "birth name" => new EncodedSPO("", "wdt:P1477"),
                "date of birth" => new EncodedSPO("", "wdt:P569"),
                "occupation" => new EncodedSPO("", "wdt:P106"),
                "continent" => new EncodedSPO("", "p:P30/ps:P30"),
                "official language" => new EncodedSPO("", "p:P37/ps:P37"),
                "capital" => new EncodedSPO("", "p:P36/ps:P36"),
                "population" => new EncodedSPO("", "p:P1082/ps:P1082"),
                _ => new EncodedSPO("", "wdt:P31"), //wdt:P31 means instance of 
            };
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
