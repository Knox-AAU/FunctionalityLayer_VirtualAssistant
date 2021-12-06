using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualAssistantBusinessLogic.KnowledgeGraph;

namespace VirtualAssistantBusinessLogic.SparQL
{
    /// <summary>
    /// SparQL Builder getting relevant information for persons
    /// </summary>
    public class PersonSparQLBuilder : SparQLBuilder
    {
        public PersonSparQLBuilder(ISPOEncoder spoEncoder) : base(spoEncoder) { }

        public override string GetSparQLTemplate(string subject, SparQLSelect sparQLSelect)
        {
            return sparQLSelect
                        .Select("Type", "Continent", "Official_language", "Capital", "Population")
                        .Where()
                            .EncodePredicates("Type", "Continent", "Official language", "Capital", "Population")
                            .SubjectIs(subject).PredicateIs("Type").GetObjectIn("Type")
                            .SubjectIs(subject).PredicateIs("Continent").GetObjectIn("Continent")
                            .SubjectIs(subject).PredicateIs("Official language").GetObjectIn("Official_language")
                            .SubjectIs(subject).PredicateIs("Capital").GetObjectIn("Capital")
                            .SubjectIs(subject).PredicateIs("Population").GetObjectIn("Population")
                        .ToString();
        }
    }
}
