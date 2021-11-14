using System.Collections.Generic; // Require by dictionary
using NUnit.Framework;
using VirtualAssistantBusinessLogic.SparQL;
using VirtualAssistantBusinessLogic.KnowledgeGraph;

namespace VirtualAssistantBusinessLogicTests
{
    public class IntegrationTest
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void it_can_get_response_from_knowledge_graph_api()
        {
            string query = "SELECT DISTINCT *"
                         + "WHERE {"
                         + "dbr:Joe_Biden dbo:birthName ?birthName ;"
                         + "              dbo:birthDate ?birthDate ;"
                         + "              dbp:birthPlace ?birthPlace ;"
                         + "              dbp:occupation ?occupation ;"
                         + "              dbp:office ?office ;"
                         + "              dbp:order ?order ."
                         + "}";

            SparQLConnection connection = new SparQLConnection();
            Dictionary<string, List<string>> data = connection.ExecuteQuery(query);

            StringAssert.IsMatch("Joseph Robinette Biden Jr.", data["birthName"][0], data["birthName"][0]);
            StringAssert.IsMatch("1942-11-20", data["birthDate"][0], data["birthDate"][0]);
            StringAssert.IsMatch("Scranton, Pennsylvania, U.S.", data["birthPlace"][0], data["birthPlace"][0]);
            StringAssert.IsMatch("Politician", data["occupation"][1], data["occupation"][1]);
            StringAssert.IsMatch("author", data["occupation"][2], data["occupation"][2]);
            StringAssert.IsMatch("lawyer", data["occupation"][3], data["occupation"][3]);
            
            StringAssert.IsMatch("Vice President of the United States", data["office"][0], data["office"][0]); // Why are these two opposite? President should be before Vise President?!?
            StringAssert.IsMatch("President of the United States", data["office"][1], data["office"][1]);
            StringAssert.IsMatch("46", data["order"][0], data["order"][0]);
            StringAssert.IsMatch("47", data["order"][1], data["order"][1]);
        }
    }
}
