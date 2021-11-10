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
            EncodedSPOs = new List<EncodedSPO>();
        }
        private string SubjectString { get; set; } = "";
        private string ObjectString { get; set; } = "";
        private string PredicateString { get; set; } = "";
        private List<string> Conditions { get; set; }
        private const string labelServiceSparQL = "SERVICE wikibase:label { bd:serviceParam wikibase:language \"en\". }";
        public SparQLSelect SparQLSelect;
        private List<EncodedSPO> EncodedSPOs { get; set; }

        public SparQLWhere SubjectIs(EncodedSPO subject)
        {
            if (EncodedSPOs.Contains(subject))
            {
                SubjectString = subject.Name;
            }
            else
            {
                SubjectString = $"{subject.Triplet}{subject.Name}";
                EncodedSPOs.Add(subject);
            }
            
            if (TripletIsDone())
            {
                AddCondition();
            }
            return this;
        }
        public SparQLWhere PredicateIs(EncodedSPO predicate)
        {
            if (EncodedSPOs.Contains(predicate))
            {
                PredicateString = predicate.Name;
            }
            else
            {
                PredicateString = $"{predicate.Triplet}{predicate.Name}";
                EncodedSPOs.Add(predicate);
            }
            if (TripletIsDone())
            {
                AddCondition();
            }
            return this;
        }

        public SparQLWhere ObjectIs(EncodedSPO obj)
        {
            if (EncodedSPOs.Contains(obj))
            {
                SubjectString = obj.Name;
            }
            else
            {
                SubjectString = $"{obj.Triplet}{obj.Name}";
                EncodedSPOs.Add(obj);
            }
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

        public override string ToString()
        {
            if (SubjectString != "" && PredicateString != "" && ObjectString != "")
            {
                throw new Exception("WHERE triplet is not done");//TODO more specific exception
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(SparQLSelect.ToString());
            sb.Append("WHERE {");
            foreach(string str in Conditions)
            {
                sb.Append(" ");
                sb.Append(str);
            }
            sb.Append(" "+labelServiceSparQL);
            sb.Append("}");
            return sb.ToString();
        }
    }
}
