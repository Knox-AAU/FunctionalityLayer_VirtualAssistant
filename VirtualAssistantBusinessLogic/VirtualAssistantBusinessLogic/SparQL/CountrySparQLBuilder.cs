using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualAssistantBusinessLogic.KnowledgeGraph;

namespace VirtualAssistantBusinessLogic.SparQL
{
    public class CountrySparQLBuilder : SparQLBuilder
    {
        public CountrySparQLBuilder(ISPOEncoder spoEncoder) : base(spoEncoder) { }
        public override string Build()
        {
            string subject = Query;

            SparQLSelect sparQLSelect = new SparQLSelect();

            //Return the SparQL string
            return sparQLSelect //TODO: make function for this
                        .Select("Type", "Continent", "Official_language", "Capital", "Population")
                        .Where()
                            .EncodePredicates("Type", "Continent", "Official language", "Capital", "Population")
                            .SubjectIs(subject).PredicateIs("Type").ObjectAs("Type")
                            .SubjectIs(subject).PredicateIs("Continent").ObjectAs("Continent")
                            .SubjectIs(subject).PredicateIs("Official language").ObjectAs("Official_language")
                            .SubjectIs(subject).PredicateIs("Capital").ObjectAs("Capital")
                            .SubjectIs(subject).PredicateIs("Population").ObjectAs("Population")
                        .ToString();
        }
    }
}
