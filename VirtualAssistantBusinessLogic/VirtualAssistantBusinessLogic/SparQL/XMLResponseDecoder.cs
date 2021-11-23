using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace VirtualAssistantBusinessLogic.SparQL
{
    /// <summary>
    /// Decoder for decoding XML responses from a sparql connection.
    /// It assumes the format of the XML is the same as wikidata responses.
    /// </summary>
    public class XMLResponseDecoder : IResponseDecoder
    {
        /// <summary>
        /// Decodes the XML read from the stream
        /// </summary>
        /// <param name="stream">XML stream</param>
        /// <returns>Decoded response</returns>
        public Dictionary<string, Dictionary<string, List<string>>> Decode(Stream stream)
        {
            var results = new Dictionary<string, Dictionary<string, List<string>>>();
            XmlReader xmlReader = XmlReader.Create(stream);

            //Read each result
            while (xmlReader.ReadToFollowing("result"))
            {
                string id = "_";
                //In this result - read each binding
                while (xmlReader.ReadToFollowing("binding"))
                {
                    //Read the bindings name (the variable from the select for this binding)
                    string key = xmlReader.GetAttribute("name");
                    if (!key.Contains("Label"))//We use the wikidata label service, except for the identifying variable
                    {
                        id = FindId(xmlReader);
                        //If the id is new
                        if (!results.ContainsKey(id))
                        {
                            //Add a new key value pair for the id
                            results[id] = new Dictionary<string, List<string>>();
                        }
                    }
                    else
                    {
                        string sanitizedKey = FindAndAddKey(results, id, key);
                        AddValueToKey(results, xmlReader, id, sanitizedKey);
                    }
                }
            }
            return results;
        }
        /// <summary>
        /// Finds the key from the XML parsing and checks whether the key already exists.
        /// If it does not exist, it is added to results array
        /// Either way we return the found key
        /// </summary>
        /// <param name="results">the results dict to add keys to</param>
        /// <param name="id"> id found from previous step</param>
        /// <param name="key"> unsanitized key with label</param>
        /// <returns>key with label part removed</returns>
        private static string FindAndAddKey(Dictionary<string, Dictionary<string, List<string>>> results, string id, string key)
        {
            if (id == "_" && !results.ContainsKey(id))
            {
                results[id] = new Dictionary<string, List<string>>();
            }
            //Remove Label from key
            key = key.Substring(0, key.Length - "Label".Length);
            //If the key is new
            if (!results[id].ContainsKey(key))
            {
                results[id][key] = new List<string>();
            }
            return key;
        }

        /// <summary>
        /// Finds the value to a key in a XML file.
        /// </summary>
        /// <param name="results"></param>
        /// <param name="xmlReader"></param>
        /// <param name="id"></param>
        /// <param name="key"></param>
        private static void AddValueToKey(Dictionary<string, Dictionary<string, List<string>>> results, XmlReader xmlReader, string id, string key)
        {
            //The bindings have a literal subelement
            xmlReader.ReadToDescendant("literal");
            //Read the literal value for the key
            string value = xmlReader.ReadElementContentAsString();
            //If not already added (distinct)
            if (!results[id][key].Contains(value))
            {
                results[id][key].Add(value);
            }
        }

        private static string FindId(XmlReader xmlReader)
        {
            string id;
            Regex filter = new(@"(Q[0-9]+)");//Entities have a .../entity/Q... uri
            var match = filter.Match(xmlReader.ReadInnerXml());
            if (match.Success)
            {
                id = "wd:" + match.Value;//TODO check if wd is always the case or if some needs another prefix based on the uri
            }
            else
            {
                id = "_";//If the id can't be found, collect it in this id
            }
            return id;
        }
    }
}
