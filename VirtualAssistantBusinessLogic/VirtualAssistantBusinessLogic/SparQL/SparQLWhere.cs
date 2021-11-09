using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAssistantBusinessLogic.SparQL
{
    public class SparQLWhere
    {
        public SparQLWhere(SparQLSelect sparQLSelect)
        {
            Conditions = new List<string>();
            SparQLSelect = sparQLSelect;
        }
        private string SubjectString { get; set; } = "";
        private string ObjectString { get; set; } = "";
        private string PredicateString { get; set; } = "";
        private List<string> Conditions { get; set; }
        private const string labelServiceSparQL = "SERVICE wikibase:label { bd:serviceParam wikibase:language \"en\". }";
        public SparQLSelect SparQLSelect;

        public SparQLWhere SubjectIs(string subject)
        {
            SubjectString = subject;
            if (TripletIsDone())
            {
                AddCondition();
            }
            return this;
        }
        public SparQLWhere PredicateIs(string predicate)
        {
            PredicateString = predicate;
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

        public override string ToString()
        {
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
