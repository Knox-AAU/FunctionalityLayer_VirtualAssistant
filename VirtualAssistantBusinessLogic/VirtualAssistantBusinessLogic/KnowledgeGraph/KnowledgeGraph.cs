using System.Net.Http;
using System.Web;
using System.Net;
using System.IO;
using System;
using System.Xml;

namespace VirtualAssistantBusinessLogic.KnowledgeGraph
{
    public class KnowledgeGraph
    {
        public KnowledgeGraphNode FindNode(string name)
        {
            //TODO split into subject predicate and object
            string subject = name;
            string predicate = name;
            string obj = name;

            SPOEncoder encoder = new SPOEncoder();
            string encodedSubject = encoder.EncodeSubject(subject);
            string encodedPredicate = encoder.EncodePredicate(predicate);
            string encodedObject = encoder.EncodeObject(obj);

            //TODO determine which SparQL template to use (fx person template if subject is a name)
            string template = "person";
            string sparql = new SparQLTemplates().GetSparQL(template, encodedSubject, encodedPredicate, encodedObject);

            //Call knowledge graph API
            string responseString = CallKnowledgeGraphAPI(sparql);

            

            KnowledgeGraphNode node = new KnowledgeGraphNode();
            node.Name = responseString;
            return node;
        }

        private string CallKnowledgeGraphAPI(string sparql)
        {
            string responseString = string.Empty;
            string baseUrl = @"https://query.wikidata.org/sparql?query=";


            string url = baseUrl + HttpUtility.UrlEncode(sparql);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36";//If the UserAgent is not set a 403 Forbidden is received from wiki.
            //request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            {
                XmlReader xmlReader = XmlReader.Create(stream);
                while (xmlReader.ReadToFollowing("literal"))
                {
                    responseString += xmlReader.Value;
                    responseString += "\n";
                }
            }

            Console.WriteLine(responseString);
            return responseString;
        }
    }
}
