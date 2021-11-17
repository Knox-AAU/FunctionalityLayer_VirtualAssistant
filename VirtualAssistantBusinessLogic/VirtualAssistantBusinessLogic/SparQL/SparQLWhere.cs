using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualAssistantBusinessLogic.KnowledgeGraph;

namespace VirtualAssistantBusinessLogic.SparQL
{
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

        public SparQLWhere ObjectIs(string obj)
        {
            ObjectString = obj;
            if (TripletIsDone())
            {
                AddCondition();
            }
            return this;
        }
        
        public SparQLWhere SubjectAs(string subject)
        {
            SubjectString = $"?{subject}";
            if (TripletIsDone())
            {
                AddCondition();
            }
            return this;
        }
        
        public SparQLWhere PredicateAs(string predicate)
        {
            PredicateString = $"?{predicate}";
            if (TripletIsDone())
            {
                AddCondition();
            }
            return this;
        }

        public SparQLWhere ObjectAs(string obj)
        {
            ObjectString = $"?{obj}";
            if (TripletIsDone())
            {
                AddCondition();
            }
            return this;
        }

        private bool TripletIsDone()
        {
            if (SubjectString != "" && PredicateString != "" && ObjectString != "")
            {
                return true;
            }
            return false;
        }

        private void AddCondition()
        {
            Conditions.Add($"{SubjectString} {PredicateString} {ObjectString}. ");

            //Reset SPO elements
            SubjectString = "";
            PredicateString = "";
            ObjectString = "";
        }

        public SparQLWhere EncodePredicates(params string[] values)
        {
            ISPOEncoder encoder = SparQLSelect.SPOEncoder;
            foreach (string value in values)
            {
                EncodedSPOs[value] = encoder.EncodePredicate(value);
            }
            return this;
        }

        public override string ToString()
        {
            if (SubjectString != "" && PredicateString != "" && ObjectString != "")
            {
                //The strings should all be empty or a partial triplet is in progress
                throw new Exception("WHERE triplet is not done");//TODO máybe more specific
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
