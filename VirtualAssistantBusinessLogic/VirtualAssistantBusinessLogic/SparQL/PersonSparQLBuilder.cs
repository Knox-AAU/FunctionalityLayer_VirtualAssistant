using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAssistantBusinessLogic.SparQL
{
    public class PersonSparQLBuilder : SparQLBuilder
    {
        public PersonSparQLBuilder(string query) : base(query)
        {
        }

        public override string ToString()
        {
            //TODO split into subject and predicate and lemmatize
            string subject = Query;

            string encodedSubject = SPOEncoder.EncodeSubject(subject);

            //Return the SparQL string
            return Select("Spouse", "Gender")
                    .Where()
                        .SubjectIs(encodedSubject).PredicateIs(SPOEncoder.EncodePredicate("Spouse")).ObjectAs("Spouse")
                        .SubjectIs(encodedSubject).PredicateIs(SPOEncoder.EncodePredicate("Gender")).ObjectAs("Gender")
                    .ToString();
        }
    }
}
