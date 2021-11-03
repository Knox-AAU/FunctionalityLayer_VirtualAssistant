using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAssistantBusinessLogic
{
    public class SparQLTemplates
    {
        public string GetSparQL(string template, string subject, string predicate, string obj)
        {
            return $"SELECT ?child ?childLabel WHERE {{ ?child wdt:P26 {subject}. SERVICE wikibase:label {{ bd:serviceParam wikibase:language \"en\". }}}}";//Gets wife(s) [AUTO_LANGUAGE] er ikke læsbart
        }
    }
}
