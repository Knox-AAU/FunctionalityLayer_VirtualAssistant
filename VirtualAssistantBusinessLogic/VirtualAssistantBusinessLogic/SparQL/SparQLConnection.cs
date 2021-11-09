using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web;
using System.Net;
using System.IO;

namespace VirtualAssistantBusinessLogic.SparQL
{
    public class SparQLConnection
    {
        public Dictionary<string, List<string>> ExecuteQuery(string query)
        {
            Dictionary<string, List<string>> results = new Dictionary<string, List<string>>();
            string baseUrl = @"https://query.wikidata.org/sparql?query=";


            string url = baseUrl + HttpUtility.UrlEncode(query);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36";//If the UserAgent is not set a 403 Forbidden is received from wiki.
            //request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            {
                XmlReader xmlReader = XmlReader.Create(stream);
                while (xmlReader.ReadToFollowing("result"))
                {
                    while (xmlReader.ReadToFollowing("binding"))
                    {
                        string key = xmlReader.GetAttribute("name");
                        xmlReader.ReadToDescendant("literal");
                        key = key.Substring(0, key.Length - "Label".Length);
                        if (!results.ContainsKey(key))
                        {
                            results[key] = new List<string>();
                        }
                        results[key].Add(xmlReader.ReadElementContentAsString());
                    }
                }
            }

            Console.WriteLine(results);
            return results;
        }
    }
}
