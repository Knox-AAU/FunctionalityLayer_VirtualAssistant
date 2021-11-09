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
            return "wd:Q22686";//Donald Trump
        }
        public String EncodePredicate(string predicate)
        {
            return "wdt:P26";//Husband
        }
        public String EncodeObject(string obj)
        {
            return obj;
        }
    }
}
