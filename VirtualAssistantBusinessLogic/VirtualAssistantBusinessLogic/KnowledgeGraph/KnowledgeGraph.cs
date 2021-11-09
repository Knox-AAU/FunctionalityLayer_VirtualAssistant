using System;
using VirtualAssistantBusinessLogic.SparQL;
using System.Collections.Generic;

namespace VirtualAssistantBusinessLogic.KnowledgeGraph
{
    public class KnowledgeGraph
    {
        public List<KnowledgeGraphNode> FindNodes(string query)
        {
            SparQLConnection sparqlConnection = new SparQLConnection();
            //TODO figure out which builder to create
            SparQLBuilder sparqlBuilder = new PersonSparQLBuilder(query);
            string sparqlQuery = sparqlBuilder.ToString();
            Dictionary<string, List<string>> result = sparqlConnection.ExecuteQuery(sparqlQuery);

            KnowledgeGraphNode node = new KnowledgeGraphNode();
            node.Name = query;//TODO figure out how to get the nodes own name (fx The node of "Barack Obama's wife" should be called "Michele Obama")
            node.Information = result;
            return node;
        }
    }
}
