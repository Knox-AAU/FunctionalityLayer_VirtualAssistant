using System.Collections.Generic; // Required by Dictionary and List
using System.Xml; // Required by XmlReader
using System.Web; // Resuited by HttpUtility
using System.Net; // Required by HttpWebRequest & HttpWebResponse
using System.IO; // Required by Stream

namespace VirtualAssistantBusinessLogic.SparQL
{
    public class SparQLConnection
    {
        public Dictionary<string, List<string>> ExecuteQuery(string query)
        {
            string url = "https://dbpedia.org/sparql";
            string queryUrl = url + "?&query=" + HttpUtility.UrlEncode(query) + "&format=xml";

            Stream stream = WebRequest.Create(queryUrl).GetResponse().GetResponseStream();
            XmlReader xml = XmlReader.Create(stream);

            Dictionary<string, List<string>> data = new Dictionary<string, List<string>>();

            while (xml.ReadToFollowing("binding"))
            {
                string prop = xml[0];

                xml.ReadToDescendant("literal");
                
                List<string> obj = new List<string>();
                obj.Add(xml.ReadElementContentAsString());

                if(data.ContainsKey(prop))
                {
                    if(!data[prop].Contains(obj[0])) // If list in dictionary does not contain the returned object.
                    {
                        data[prop].Add(obj[0]);
                    }
                } else {
                    data.Add(prop, obj);
                }
            }
            return data;
        }
    }
}
