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
    /// WILL ONLY WORK FOR WIKIDATA IN THE CURRENT STATE (because of the labelServiceSparQL)
    /// </summary>
    public class SparQLWhere
    {
        public SparQLWhere(SparQLSelect sparQLSelect)
        {
            Conditions = new List<string>();
            SparQLSelect = sparQLSelect;
            EncodedSPOs = new Dictionary<string, EncodedSPO>();
            if (SparQLSelect.FromSubject != null)
            {
                EncodedSPOs.Add(SparQLSelect.FromSubjectRaw, SparQLSelect.FromSubject);
            }
        }
        public string SubjectString { get; private set; } = "";
        public string ObjectString { get; private set; } = "";
        public string PredicateString { get; private set; } = "";
        public List<string> Conditions { get; private set; }
        private const string labelServiceSparQL = "SERVICE wikibase:label { bd:serviceParam wikibase:language \"en\". }";
        public SparQLSelect SparQLSelect;
        public Dictionary<string, EncodedSPO> EncodedSPOs { get; private set; }

        /// <summary>
        /// Specifies the subject of the sparql triplet as being equal to the
        /// input subject.
        /// </summary>
        /// <param name="subject">Subject of the sparql triplet</param>
        /// <returns>SparQLWhere object</returns>
        public SparQLWhere SubjectIs(string subject)
        {
            if (SubjectString != "") throw new ArgumentException("Subject in query has already been set. Finish current condition first");

            // Subject can either be encoded (if ID is unknown) or an ID
            SubjectString = EncodedSPOs.ContainsKey(subject) ? EncodedSPOs[subject].Name : subject;

            if (IsTripletDone())
            {
                AddCondition();
            }
            return this;
        }

        /// <summary>
        /// Specifies the predicate of the sparql triplet as being equal to the
        /// input parameter.
        /// </summary>
        /// <param name="predicate">Predicate of the sparql triplet</param>
        /// <returns>SparQLWhere object</returns>
        public SparQLWhere PredicateIs(string predicate)
        {
            if (PredicateString != "") throw new ArgumentException("Predicate in query has already been set. Finish current condition first");

            if (!EncodedSPOs.ContainsKey(predicate))
            {
                throw new KeyNotFoundException("Encoded SPO does not include the predicate key");
            }

            PredicateString = EncodedSPOs[predicate].Name;
            if (IsTripletDone())
            {
                AddCondition();
            }
            return this;
        }

        /// <summary>
        /// Specifies the name of the variable
        /// </summary>
        /// <param name="obj">name of the object variable</param>
        /// <returns>SparQLWhere object</returns>
        public SparQLWhere GetObjectIn(string obj)
        {
            if (ObjectString != "") throw new ArgumentException("Object in query has already been set. Finish current condition first");

            ObjectString = $"?{obj}";
            if (IsTripletDone())
            {
                AddCondition();
            }
            return this;
        }

        /// <summary>
        /// Checks wether the subject, predicate, and the object all have a value
        /// </summary>
        /// <returns>whether triplet is done</returns>
        private bool IsTripletDone()
        {
            if (SubjectString != "" && PredicateString != "" && ObjectString != "")
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds the current triplet to conditions so they can be used when building the query
        /// </summary>
        private void AddCondition()
        {
            Conditions.Add($"{SubjectString} {PredicateString} {ObjectString}. ");

            //Reset SPO elements
            SubjectString = "";
            PredicateString = "";
            ObjectString = "";
        }

        /// <summary>
        /// Encodes the values so they can be used in triplets that needs encoding.
        /// Must be called BEFORE adding triplets that needs encoding
        /// </summary>
        /// <param name="values">values to be encoded</param>
        /// <returns></returns>
        public SparQLWhere EncodePredicates(params string[] values)
        {
            ISPOEncoder encoder = SparQLSelect.SPOEncoder;
            foreach (string value in values)
            {
                EncodedSPOs[value] = encoder.EncodePredicate(value);
            }
            return this;
        }

        /// <summary>
        /// Builds the query
        /// Note the query is build using UNION for each condition,
        /// so that as much information as possible is returned by the query,
        /// but if some information is missing the other information is still returned.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (SubjectString != "" && PredicateString != "" && ObjectString != "")
            {
                //The strings should all be empty or a partial triplet is in progress
                throw new Exception("WHERE triplet is not done");
            }
            StringBuilder sb = new();
            //Since we use fluent we need to include the select's ToString as well
            sb.Append(SparQLSelect.ToString());
            //Start the WHERE
            sb.Append("WHERE {");
            //Any encoded spos must be first (before the unions)
            foreach (EncodedSPO encodedSPO in EncodedSPOs.Values)
            {
                sb.Append(encodedSPO.Triplet);
            }
            bool firstInUnion = true;
            foreach (string str in Conditions)
            {
                if (!firstInUnion)
                {
                    sb.Append("UNION");
                }
                sb.Append(" {");
                sb.Append(str);
                sb.Append('}');
                firstInUnion = false;
            }
            sb.Append(" " + labelServiceSparQL);
            sb.Append('}');
            return sb.ToString();
        }
    }
}
