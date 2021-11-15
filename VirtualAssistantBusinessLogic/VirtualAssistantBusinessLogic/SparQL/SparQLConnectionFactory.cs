using System;

namespace VirtualAssistantBusinessLogic.SparQL
{
    public class SparQLConnectionFactory
    {
        private static string Connection { get; set; } = "wiki";
        public ISparQLConnection GetConnection()
        {
            switch (Connection.ToLower())
            {
                case "wiki": return new WikidataSparQLConnection();
                default: throw new NotImplementedException();
            }
        }
    }
}
