using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualAssistantBusinessLogic.KnowledgeGraph;

namespace VirtualAssistantBusinessLogic.SparQL
{
    public class SparQLBuilder
    {
        public SparQLBuilder(string query)
        {
            SPOEncoder = new SPOEncoder();
            Query = query;
        }

        private string Query { get; set; }
        private SPOEncoder SPOEncoder { get; set; }

        public SparQLBuilder DetectAndUseTemplate()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            
             //TODO split into subject and predicate and lemmatize
            string subject = Query;
            string predicate = "Wife";

            
            string encodedSubject = SPOEncoder.EncodeSubject(subject);
            string encodedPredicate = SPOEncoder.EncodePredicate(predicate);

            //Return the SparQL string
            return Select("Predicate", "Object").Where().SubjectIs(encodedSubject).PredicateAs("Predicate").ObjectAs("Object").ToString();
        }

        private SparQLSelect Select(params string[] values)
        {
            return new SparQLSelect().Select(values);
        }
    }
}
