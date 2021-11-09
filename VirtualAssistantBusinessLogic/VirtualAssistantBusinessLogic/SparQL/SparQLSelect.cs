using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAssistantBusinessLogic.SparQL
{
    public class SparQLSelect
    {
        public SparQLSelect()
        {
            Selects = new List<string>();
        }
        private List<string> Selects { get; set; }

        public SparQLSelect Select(params string[] values)
        {
            foreach (string value in values)
            {
                Selects.Add(value);
            }
            return this;
        }

        public SparQLWhere Where()
        {
            if (Selects.Count == 0)
            {
                throw new Exception("Must select something.");//TODO maybe other type of exception
            }
            return new SparQLWhere(this);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            foreach (string value in Selects)
            {
                sb.Append($"?{value}Label ");
            }
            return sb.ToString();
        }
    }
}
