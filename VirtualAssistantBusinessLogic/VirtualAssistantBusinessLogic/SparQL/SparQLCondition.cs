using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAssistantBusinessLogic.SparQL
{
    public class SparQLCondition
    {
        public SparQLCondition()
        {
            Conditions = new List<string>();
        }
        private string SubjectString { get; set; } = "";
        private string ObjectString { get; set; } = "";
        private string PredicateString { get; set; } = "";
        private List<string> Conditions { get; set; }
        private const string labelServiceSparQL = "SERVICE wikibase:label { bd:serviceParam wikibase:language \"en\". }";

        public SparQLCondition SubjectIs(string subject)
        {
            SubjectString = subject;
            return this;
        }
        public SparQLCondition PredicateIs(string predicate)
        {
            PredicateString = predicate;
            return this;
        }

        public SparQLCondition ObjectIs(string obj)
        {
            ObjectString = obj;
            return this;
        }

        /// <summary>
        /// Defines the name of the selection
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SparQLCondition As(string name)
        {
            // Determine what the condition finds (S P or O)
            string elementToFind = SPOElementToFind();
            switch (elementToFind)
            {
                case "Subject":
                    Conditions.Add($"?{name} {PredicateString} {ObjectString}.");
                    break;
                case "Predicate":
                    Conditions.Add($"{SubjectString} ?{name} {ObjectString}.");
                    break;
                case "Object":
                    Conditions.Add($"{SubjectString} {PredicateString} ?{name}.");
                    break;
                default: throw new Exception($"{elementToFind} is not valid in this context");//TODO more precise exception
            }
            //Reset SPO elements
            SubjectString = "";
            PredicateString = "";
            ObjectString = "";
            return this;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
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

        private string SPOElementToFind()
        {
            if (SubjectString == "")
            {
                if (PredicateString != "" && ObjectString != "")
                {
                    return "Subject";
                }
                else
                {
                    return "Multiple things";//TODO allow multiples or throw exception
                }
            }
            else if (PredicateString == "")
            {
                if (ObjectString != "")
                {
                    return "Predicate";
                }
                else
                {
                    return "Multiple things";//TODO allow multiples or throw exception
                }
            }
            else if (ObjectString == "")
            {
                return "Object";
            }
            else
            {
                return "All things";//TODO allow multiples or throw exception
            }
        }
    }
}
