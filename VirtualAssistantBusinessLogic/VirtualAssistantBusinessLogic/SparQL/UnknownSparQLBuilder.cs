using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualAssistantBusinessLogic.KnowledgeGraph;

namespace VirtualAssistantBusinessLogic.SparQL
{
    /// <summary>
    /// SparQL Builder to build queries for getting identifying information
    /// </summary>
    public class UnknownSparQLBuilder : SparQLBuilder
    {
        public UnknownSparQLBuilder(ISPOEncoder spoEncoder) : base(spoEncoder) { }
        public override string Build()
        {
            string subject = Query;

            SparQLSelect sparQLSelect = new SparQLSelect(SPOEncoder);
            //Return the SparQL string
            return sparQLSelect
                        .Select("Type", "Occupation", "birth_name", "date_of_birth", "Spouse")
                            .From(subject)
                        .Where()
                            .EncodePredicates("Type", "Occupation", "birth name", "date of birth", "Spouse")
                            .SubjectIs(subject).PredicateIs("Type").GetObjectIn("Type")
                            .SubjectIs(subject).PredicateIs("Occupation").GetObjectIn("Occupation")
                            .SubjectIs(subject).PredicateIs("birth name").GetObjectIn("birth_name")
                            .SubjectIs(subject).PredicateIs("date of birth").GetObjectIn("date_of_birth")
                            .SubjectIs(subject).PredicateIs("Spouse").GetObjectIn("Spouse")
                        .ToString();
        }
    }
}
