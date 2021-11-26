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
        public override string Build()
        {
            string subject = Query;

            SparQLSelect sparQLSelect = new(SPOEncoder);

            //Return the SparQL string
            return sparQLSelect //TODO: make function for this
                        .Select("Type", "Continent", "Official_language", "Capital", "Population")
                        .Where()
                            .EncodePredicates("Type", "Continent", "Official language", "Capital", "Population")
                            .SubjectIs(subject).PredicateIs("Type").ObjectIs("Type")
                            .SubjectIs(subject).PredicateIs("Continent").ObjectIs("Continent")
                            .SubjectIs(subject).PredicateIs("Official language").ObjectIs("Official_language")
                            .SubjectIs(subject).PredicateIs("Capital").ObjectIs("Capital")
                            .SubjectIs(subject).PredicateIs("Population").ObjectIs("Population")
                        .ToString();
        }
    }
}
