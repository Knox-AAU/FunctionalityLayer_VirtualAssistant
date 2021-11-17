﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualAssistantBusinessLogic.SparQL
{
    /// <summary>
    /// Interface for SparQL Connections
    /// </summary>
    public interface ISparQLConnection
    {
        public SparQLBuilder GetSparQLBuilder(string type);
        public Dictionary<string, Dictionary<string, List<string>>> ExecuteQuery(string query);
    }
}
