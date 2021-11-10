using System;
using VirtualAssistantBusinessLogic.SparQL;
using System.Collections.Generic;

namespace VirtualAssistantBusinessLogic.KnowledgeGraph
{
    public class KnowledgeGraph
    {
        public KnowledgeGraphNode FindNodeInformation(KnowledgeGraphNode node)
        {
            SparQLConnection sparqlConnection = new SparQLConnection();
            SparQLBuilder sparqlBuilder = GetSparQLBuilder(node.Information["Type"][0]);
            sparqlBuilder.Query = node.Id;
            string sparqlQuery = sparqlBuilder.ToString();
            Dictionary<string, Dictionary<string, List<string>>> result = sparqlConnection.ExecuteQuery(sparqlQuery);

            foreach(var kvp in result)
            {
                node.Information = kvp.Value;
                break;//We know there is only one, so break after reading it
            }
            return node;
        }

        public List<KnowledgeGraphNode> FindNodes(string query)
        {
            SparQLConnection sparqlConnection = new SparQLConnection();
            SparQLBuilder sparqlBuilder = GetSparQLBuilder("");
            sparqlBuilder.Query = query;
            string sparqlQuery = sparqlBuilder.ToString();
            Dictionary<string, Dictionary<string, List<string>>> results = sparqlConnection.ExecuteQuery(sparqlQuery);

            List<KnowledgeGraphNode> nodeList = new List<KnowledgeGraphNode>();
            foreach (var kvp in results)
            {
                KnowledgeGraphNode node = new KnowledgeGraphNode();
                node.Name = kvp.Key;//TODO figure out how to get the nodes own name (fx The node of "Barack Obama's wife" should be called "Michele Obama")
                node.Information = kvp.Value;

                nodeList.Add(node);
            }
            
            return nodeList;
        }

        private SparQLBuilder GetSparQLBuilder(string type)
        {
            switch (type)
            {
                case "": return new UnknownSparQLBuilder();
                case "Human": return new PersonSparQLBuilder();
                default: return new SparQLBuilder();
            }
        }
    }
}
