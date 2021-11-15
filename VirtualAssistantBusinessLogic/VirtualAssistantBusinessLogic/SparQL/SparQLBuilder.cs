using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualAssistantBusinessLogic.KnowledgeGraph;

namespace VirtualAssistantBusinessLogic.SparQL
{
    public class SparQLBuilder : ISparQLBuilder
    {
        public SparQLBuilder(ISPOEncoder spoEncoder)
        {
            SPOEncoder = spoEncoder;
        }

        public string Query { get; set; }
        protected ISPOEncoder SPOEncoder { get; set; }

        public virtual string Build()
        {
            //TODO split into subject and predicate and lemmatize
            string subject = Query;
            throw new NotImplementedException();
            /*
            EncodedSPO encodedSubject = SPOEncoder.EncodeSubject(subject);

            SparQLSelect sparQLSelect = new SparQLSelect();
            //Return the SparQL string
            return sparQLSelect.From(encodedSubject)//TODO change from to the initial queries resulting start node
                        .Select("Predicate", "Object")
                        .Where()
                            .SubjectIs(encodedSubject).PredicateAs("Predicate").ObjectAs("Object")
                        .ToString();*/
        }
    }
}
