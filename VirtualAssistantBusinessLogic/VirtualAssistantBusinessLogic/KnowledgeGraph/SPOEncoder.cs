using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAssistantBusinessLogic.KnowledgeGraph
{
    public class SPOEncoder
    {
        public String EncodeSubject(string subject)
        {
            /*
             SELECT DISTINCT ?sLabel ?sDescription ?propLabel
                WHERE {
                  ?s wdt:P31 wd:Q5. # Is an instance "Human"
                  ?s ?p "Donald Trump"@en . # Is object "Donald Trump"
                  wd:Q5 wdt:P1963 ?prop .
  
                  SERVICE wikibase:label { bd:serviceParam wikibase:language "[AUTO_LANGUAGE]". }
                }
             */
            //return "wd:Q22686";//Donald Trump
            return $"?s ?p \"{subject}\"@en . ?s ";
        }
        public String EncodePredicate(string predicate)
        {
            switch (predicate.ToLower())
            {
                case "spouse": return "wdt:P26";
                default: return "wdt:P31";
            }
        }
        public String EncodeObject(string obj)
        {
            return obj;
        }
    }
}
