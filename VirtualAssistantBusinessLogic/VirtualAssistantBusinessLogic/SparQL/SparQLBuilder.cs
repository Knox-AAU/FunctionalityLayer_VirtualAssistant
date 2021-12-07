using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualAssistantBusinessLogic.KnowledgeGraph;

namespace VirtualAssistantBusinessLogic.SparQL
{
    /// <summary>
    /// Abstract class for SparQL Builders
    /// </summary>
    public abstract class SparQLBuilder : ISparQLBuilder
    {
        public SparQLBuilder(ISPOEncoder spoEncoder)
        {
            SPOEncoder = spoEncoder;
        }

        public string Query { get; set; }
        protected ISPOEncoder SPOEncoder { get; set; }

        public string[] WantedProperties { get; set; }

        /// <summary>
        /// Method for building the query
        /// </summary>
        /// <returns></returns>
        public virtual string Build()
        {
            string subject = Query;

            SparQLSelect sparQLSelect = new(SPOEncoder);
            //Return the SparQL string
            return GetSparQLQuery(subject, sparQLSelect);
        }

        /// <summary>
        /// Method for getting the SparQL using the template
        /// </summary>
        /// <returns>SparQL query in string format.</returns>
        public virtual string GetSparQLQuery(string subject, SparQLSelect sparQLSelect)
        {
            return sparQLSelect
                        .Select("Type")
                        .Where()
                            .EncodePredicates("Type")
                            .SubjectIs(subject).PredicateIs("Type").GetObjectIn("Type")
                        .ToString();
        }
    }
}
