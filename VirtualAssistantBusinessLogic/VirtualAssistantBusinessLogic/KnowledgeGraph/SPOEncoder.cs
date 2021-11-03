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
            throw new Exception("Not implemented");
        }
        public String EncodeObject(string obj)
        {
            throw new Exception("Not implemented");
        }
    }
}
