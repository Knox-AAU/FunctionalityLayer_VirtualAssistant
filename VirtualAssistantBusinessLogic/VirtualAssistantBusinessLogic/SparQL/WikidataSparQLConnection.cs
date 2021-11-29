using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using VirtualAssistantBusinessLogic.KnowledgeGraph;

namespace VirtualAssistantBusinessLogic.SparQL
{
    /// <summary>
    /// SparQL connection to the wikidata sparql API
    /// </summary>
    public class WikidataSparQLConnection : ISparQLConnection
    {
        public WikidataSparQLConnection()
        {
            ResponseDecoder = new XMLResponseDecoder();
            SupportedTypes = new List<string> {
                "", // Unknown entity - used to identify the entity
                "human",
                "country"
            };
        }

        private IResponseDecoder ResponseDecoder { get; set; }
        public List<string> SupportedTypes { get; private set; }

        /// <summary>
        /// Executes the given query and returns the response decoded
        /// </summary>
        /// <param name="query">sparql query</param>
        /// <returns>decoded API response</returns>
        public Dictionary<string, Dictionary<string, List<string>>> ExecuteQuery(string query)
        {
            string baseUrl = @"https://query.wikidata.org/sparql?query=";
            string url = baseUrl + HttpUtility.UrlEncode(query);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36";//If the UserAgent is not set, a 403 Forbidden is received from wiki.

            Dictionary<string, Dictionary<string, List<string>>> results;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            {
                results = ResponseDecoder.Decode(stream);
            }

            return results;
        }

        /// <summary>
        /// Returns the SparQLBuilder that builds queries for this connection
        /// based on the template from the given type
        /// </summary>
        /// <param name="type">type for template choice</param>
        /// <returns>SparQL Builder for building queries</returns>
        public virtual SparQLBuilder GetSparQLBuilder(string type)
        {
            ISPOEncoder spoEncoder = new WikidataSPOEncoder();
            return type.ToLower() switch
            {
                "" => new UnknownSparQLBuilder(spoEncoder),
                "human" => new PersonSparQLBuilder(spoEncoder),
                "country" => new CountrySparQLBuilder(spoEncoder),
                _ => throw new ArgumentException($"{type} has no SparQLBuilder associated."),
            };
        }

        public IEnumerable<string> SupportedTypesIntersection(List<string> strings)
        {
            for(int i = 0; i < strings.Count; ++i)
            {
                strings[i] = strings[i].ToLower();
            }
            return SupportedTypes.Intersect(strings);
        }
    }
}
