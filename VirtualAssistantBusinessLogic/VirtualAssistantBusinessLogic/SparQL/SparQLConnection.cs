using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace VirtualAssistantBusinessLogic.SparQL
{
    public class SparQLConnection
    {
        public Dictionary<string, Dictionary<string, List<string>>> ExecuteQuery(string query)
        {
            Dictionary<string, Dictionary<string, List<string>>> results = new Dictionary<string, Dictionary<string, List<string>>>();
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
                    string id = "";
                    while (xmlReader.ReadToFollowing("binding"))
                    {
                        string key = xmlReader.GetAttribute("name");
                        if (key.Contains("Label")) {
                            //Remove Label from key
                            key = key.Substring(0, key.Length - "Label".Length);
                            if (!results[id].ContainsKey(key))
                            {
                                results[id][key] = new List<string>();
                            }
                            xmlReader.ReadToDescendant("literal");
                            results[id][key].Add(xmlReader.ReadElementContentAsString());
                        }
                        else
                        {
                            Regex filter = new Regex(@"(Q[0-9]+)");
                            var match = filter.Match(xmlReader.ReadInnerXml());
                            if (match.Success)
                            {
                                id = match.Value;
                            }
                            else
                            {
                                throw new Exception("Expected id could not be read");
                            }
                            if (!results.ContainsKey(id))
                            {
                                results[id] = new Dictionary<string, List<string>>();
                            }
                        }
                    }
                }
            }

            Console.WriteLine(results);
            return results;
        }
    }
}
