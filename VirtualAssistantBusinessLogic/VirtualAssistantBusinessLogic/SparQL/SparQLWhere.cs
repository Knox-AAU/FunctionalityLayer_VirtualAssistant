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
        private string SubjectString { get; set; } = "";
        private string ObjectString { get; set; } = "";
        private string PredicateString { get; set; } = "";
        private List<string> Conditions { get; set; }
        private const string labelServiceSparQL = "SERVICE wikibase:label { bd:serviceParam wikibase:language \"en\". }";
        public SparQLSelect SparQLSelect;
        private Dictionary<string, EncodedSPO> EncodedSPOs { get; set; }

        /// <summary>
        /// Specifies the subject of the sparql triplet as being equal to the
        /// input subject.
        /// </summary>
        /// <param name="subject">Subject of the sparql triplet</param>
        /// <returns></returns>
        public SparQLWhere SubjectIs(string subject)
        {
            if (EncodedSPOs.ContainsKey(subject))
            {
                SubjectString = EncodedSPOs[subject].Name;
            }
            else
            {
                SubjectString = subject;
            }
            if (TripletIsDone())
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
        /// <returns></returns>
        public SparQLWhere PredicateIs(string predicate)
        {
            if (EncodedSPOs.ContainsKey(predicate))
            {
                PredicateString = EncodedSPOs[predicate].Name;
            }
            else
            {
                PredicateString = predicate;
            }
            if (TripletIsDone())
            {
                AddCondition();
            }
            return this;
        }

        /// <summary>
        /// Specifies the object of the sparql triplet as being equal to the
        /// input parameter.
        /// </summary>
        /// <param name="obj">Object of the sparql triplet</param>
        /// <returns></returns>
        public SparQLWhere ObjectIs(string obj)
        {
            ObjectString = obj;
            if (TripletIsDone())
            {
                AddCondition();
            }
            return this;
        }
        
        /// <summary>
        /// Specifies the name of the variable
        /// </summary>
        /// <param name="subject">name of the subject variable</param>
        /// <returns></returns>
        public SparQLWhere SubjectAs(string subject)
        {
            SubjectString = $"?{subject}";
            if (TripletIsDone())
            {
                AddCondition();
            }
            return this;
        }

        /// <summary>
        /// Specifies the name of the variable
        /// </summary>
        /// <param name="predicate">name of the predicate variable</param>
        /// <returns></returns>
        public SparQLWhere PredicateAs(string predicate)
        {
            PredicateString = $"?{predicate}";
            if (TripletIsDone())
            {
                AddCondition();
            }
            return this;
        }

        /// <summary>
        /// Specifies the name of the variable
        /// </summary>
        /// <param name="obj">name of the object variable</param>
        /// <returns></returns>
        public SparQLWhere ObjectAs(string obj)
        {
            ObjectString = $"?{obj}";
            if (TripletIsDone())
            {
                AddCondition();
            }
            return this;
        }

        /// <summary>
        /// Checks wether the subject, predicate, and the object all have a value
        /// </summary>
        /// <returns></returns>
        private bool TripletIsDone()
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
            StringBuilder sb = new StringBuilder();
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
                sb.Append("}");
                firstInUnion = false;
            }
            sb.Append(" " + labelServiceSparQL);
            sb.Append("}");
            return sb.ToString();
        }
    }
}
