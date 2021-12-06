using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualAssistantBusinessLogic.KnowledgeGraph;

namespace VirtualAssistantBusinessLogic.SparQL
{
    /// <summary>
    /// Builder class responsible for building sparql queries based on the template for information wanted about countries.
    /// </summary>
    public class CountrySparQLBuilder : SparQLBuilder
    {
        public CountrySparQLBuilder(ISPOEncoder spoEncoder) : base(spoEncoder) { }

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
