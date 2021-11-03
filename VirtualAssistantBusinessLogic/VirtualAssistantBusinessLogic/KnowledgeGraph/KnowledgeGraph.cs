using System.Text.Encodings.Web;
using System.Net.Http;

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

            //TODO determine which SparQL template to use (fx person template if subject is a name)
            string template = "person";
            string sqarql = new SparQLTemplates().GetSparQL(template, subject, predicate, obj);

            //Call knowledge graph API
            CallKnowledgeGraphAPI(sparql);
        }

        private string CallKnowledgeGraphAPI(string sparql)
        {
            string html = string.Empty;
            string url = @"https://query.wikidata.org/sparql?query=";
            
            UrlEncoder urlEncoder = new UrlEncoder();
            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            Console.WriteLine(html);
        }
    }
}
