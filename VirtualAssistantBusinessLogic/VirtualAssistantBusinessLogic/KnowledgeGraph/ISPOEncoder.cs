using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAssistantBusinessLogic.KnowledgeGraph
{
    public interface ISPOEncoder
    {
        public EncodedSPO EncodeSubject(string subject);
        public EncodedSPO EncodePredicate(string predicate);
        public EncodedSPO EncodeObject(string obj);
    }
}
