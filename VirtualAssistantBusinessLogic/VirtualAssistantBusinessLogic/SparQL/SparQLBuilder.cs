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

            SparQLCondition where = new SparQLCondition();
            string sparqlWhere = where.SubjectIs(encodedSubject).PredicateIs(encodedPredicate).As("Spouse").ToString();

            //Return the SparQL string
            return $"{Select("Spouse")} {sparqlWhere}";
        }

        private string Select(params string[] values)
        {
            if (values == null)
            {
                throw new Exception("Must select something.");//TODO maybe other type of exception
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            foreach(string value in values)
            {
                sb.Append($"?{value}Label");
            }
            return sb.ToString();
        }
    }
}
