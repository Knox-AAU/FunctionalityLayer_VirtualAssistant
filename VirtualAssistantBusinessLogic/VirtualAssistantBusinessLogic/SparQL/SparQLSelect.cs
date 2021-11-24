using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualAssistantBusinessLogic.KnowledgeGraph;

namespace VirtualAssistantBusinessLogic.SparQL
{
    /// <summary>
    /// Class for fluently building SparQL queries
    /// </summary>
    public class SparQLSelect
    {
        public SparQLSelect(ISPOEncoder spoEncoder)
        {
            Selects = new List<string>();
            SPOEncoder = spoEncoder;
        }
        public List<string> Selects { get; private set; }
        public EncodedSPO FromSubject { get; private set; }
        public string FromSubjectRaw { get; private set; }
        public ISPOEncoder SPOEncoder { get; set; }

        /// <summary>
        /// If the search in the knowledge graph must first find the start node(s)
        /// this method can be called to find the node(s) to search from.
        /// </summary>
        /// <param name="fromSubject">Name of the node(s) to start the search from</param>
        /// <returns></returns>
        public SparQLSelect From(string fromSubject)
        {
            FromSubjectRaw = fromSubject;
            FromSubject = SPOEncoder.EncodeSubject(fromSubject);
            return this;
        }

        /// <summary>
        /// Specifies which values/variables should be selected
        /// from the SparQL query
        /// </summary>
        /// <param name="values">values/variable names to be selected</param>
        /// <returns></returns>
        public SparQLSelect Select(params string[] values)
        {
            foreach (string value in values)
            {
                Selects.Add(value);
            }
            return this;
        }

        /// <summary>
        /// Called to end the select part of the fluent sparql building
        /// returning the class to fluently build the where part of the query
        /// </summary>
        /// <exception cref="Exception">Thrown if nothing is selected</exception>
        /// <returns></returns>
        public SparQLWhere Where()
        {
            if (Selects.Count == 0)
            {
                throw new CountZeroException("Must select something.");
            }
            return new SparQLWhere(this);
        }

        /// <summary>
        /// Returns the select part of the query as a string
        /// Note that calling .ToString() on chained fluent classes will 
        /// return the full query, so this function is not necessary to call
        /// but used by the chained fluent classes
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append("SELECT ");
            if (FromSubject != null)
            {
                sb.Append(FromSubject.Name);
            }
            sb.Append(' ');
            foreach (string value in Selects)
            {
                sb.Append($"?{value}Label ");
            }
            return sb.ToString();
        }
    }
}
