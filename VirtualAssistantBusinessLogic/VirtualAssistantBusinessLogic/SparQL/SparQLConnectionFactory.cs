using System;

namespace VirtualAssistantBusinessLogic.SparQL
{
    /// <summary>
    /// Factory used to create a SparQL connection based on the current connection setting
    /// </summary>
    public class SparQLConnectionFactory
    {
        private static string Connection { get; set; } = "wiki";//TODO should be moved to a build variable or read from an environment file
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
