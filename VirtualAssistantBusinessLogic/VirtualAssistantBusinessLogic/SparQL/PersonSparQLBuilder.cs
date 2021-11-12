using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualAssistantBusinessLogic.KnowledgeGraph;

namespace VirtualAssistantBusinessLogic.SparQL
{
    public class PersonSparQLBuilder : SparQLBuilder
    {
        public override string ToString()
        {
            //TODO split into subject and predicate and lemmatize
            string subject = Query;

            SparQLSelect sparQLSelect = new SparQLSelect();
            //Return the SparQL string
            return sparQLSelect
                        .Select("Type", "Occupation", "birth_name", "date_of_birth", "Spouse")
                        .Where()
                            .EncodePredicates("Type", "Occupation", "birth name", "date of birth", "Spouse")
                            .SubjectIs(subject).PredicateIs("Type").ObjectAs("Type")
                            .SubjectIs(subject).PredicateIs("Occupation").ObjectAs("Occupation")
                            .SubjectIs(subject).PredicateIs("birth name").ObjectAs("birth_name")
                            .SubjectIs(subject).PredicateIs("date of birth").ObjectAs("date_of_birth")
                            .SubjectIs(subject).PredicateIs("Spouse").ObjectAs("Spouse")
                        .ToString();
        }
    }
}
