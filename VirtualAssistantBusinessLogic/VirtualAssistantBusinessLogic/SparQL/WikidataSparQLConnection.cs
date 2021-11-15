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
    public class WikidataSparQLConnection : ISparQLConnection
    {
        public WikidataSparQLConnection()
        {
            ResponseDecoder = new XMLResponseDecoder();
        }
        private IResponseDecoder ResponseDecoder { get; set; }
        public Dictionary<string, Dictionary<string, List<string>>> ExecuteQuery(string query)
        {
            string baseUrl = @"https://query.wikidata.org/sparql?query=";
            string url = baseUrl + HttpUtility.UrlEncode(query);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36";//If the UserAgent is not set a 403 Forbidden is received from wiki.

            Dictionary<string, Dictionary<string, List<string>>> results;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            {
                results = ResponseDecoder.Decode(stream);
            }

            Console.WriteLine(results);
            return results;
        }

        public virtual SparQLBuilder GetSparQLBuilder(string type)
        {
            ISPOEncoder spoEncoder = new WikidataSPOEncoder();
            switch (type.ToLower())
            {
                case "": return new UnknownSparQLBuilder(spoEncoder);
                case "human": return new PersonSparQLBuilder(spoEncoder);
                case "country": throw new NotImplementedException();//TODO add this for the MVP
                default: return new SparQLBuilder(spoEncoder);
            }
        }
    }
}
