﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualAssistantBusinessLogic.KnowledgeGraph;

namespace VirtualAssistantBusinessLogic.SparQL
{
    /// <summary>
    /// SparQL Builder getting relevant information for persons
    /// </summary>
    public class PersonSparQLBuilder : SparQLBuilder
    {
        public PersonSparQLBuilder(ISPOEncoder spoEncoder) : base(spoEncoder) { }

        public override string GetSparQLQuery(string subject, SparQLSelect sparQLSelect)
        {
            return sparQLSelect
                        .Select("Type", "Occupation", "birth_name", "date_of_birth", "Spouse")
                        .Where()
                            .EncodePredicates("Type", "Occupation", "birth name", "date of birth", "Spouse")
                            .SubjectIs(subject).PredicateIs("Type").GetObjectIn("Type")
                            .SubjectIs(subject).PredicateIs("Occupation").GetObjectIn("Occupation")
                            .SubjectIs(subject).PredicateIs("birth name").GetObjectIn("birth_name")
                            .SubjectIs(subject).PredicateIs("date of birth").GetObjectIn("date_of_birth")
                            .SubjectIs(subject).PredicateIs("Spouse").GetObjectIn("Spouse")
                        .ToString();
        }
    }
}
