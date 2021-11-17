using System.Linq;
using VirtualAssistantBusinessLogic.SparQL;
using System.Collections.Generic;

namespace VirtualAssistantBusinessLogic.KnowledgeGraph
{
    /// <summary>
    /// Class representing the knowledge graph, that uses a SparQL Connection to
    /// find information from the knowledge graph
    /// </summary>
    public class KnowledgeGraph
    {
        public KnowledgeGraph(SparQLConnectionFactory sparQLConnectionFactory)
        {
            SparQLConnectionFactory = sparQLConnectionFactory;
        }

        private SparQLConnectionFactory SparQLConnectionFactory { get; set; }

        /// <summary>
        /// Takes a knowledge graph node and gets the relevant information for that node based on
        /// the nodes type.
        /// </summary>
        /// <param name="node">a specific knowledge graph node [returned from a call to find nodes]</param>
        /// <returns>The input node now containing information for the node</returns>
        public KnowledgeGraphNode FindNodeInformation(KnowledgeGraphNode node)
        {
            ISparQLConnection sparqlConnection = SparQLConnectionFactory.GetConnection();
            ISparQLBuilder sparqlBuilder = sparqlConnection.GetSparQLBuilder(node.Information["Type"][0]);
            sparqlBuilder.Query = node.Id;
            string sparqlQuery = sparqlBuilder.Build();
            Dictionary<string, Dictionary<string, List<string>>> results = sparqlConnection.ExecuteQuery(sparqlQuery);

            //Since we are looking for information of a specific node, we know that there is only one result
            KeyValuePair<string, Dictionary<string, List<string>>> result = results.FirstOrDefault();
            node.Information = result.Value;
            return node;
        }

        /// <summary>
        /// Finds all knowledge graph nodes that are found from the query string
        /// Eg. Chris Evans will return 21 nodes if called on the wikidata sparql connection
        /// </summary>
        /// <param name="query">Label of a node</param>
        /// <returns>List of nodes with the given label and information to identify them enough
        /// to select which node is correct, so that more information can be read for that node</returns>
        public List<KnowledgeGraphNode> FindNodes(string query)
        {
            ISparQLConnection sparqlConnection = SparQLConnectionFactory.GetConnection();
            ISparQLBuilder sparqlBuilder = sparqlConnection.GetSparQLBuilder("");
            sparqlBuilder.Query = query;
            string sparqlQuery = sparqlBuilder.Build();
            Dictionary<string, Dictionary<string, List<string>>> results = sparqlConnection.ExecuteQuery(sparqlQuery);

            //Create the node list that is returned
            List<KnowledgeGraphNode> nodeList = new List<KnowledgeGraphNode>();
            //Go through each key-value pair (kvp) from the executed query and create and append the nodes to the node list
            foreach (var kvp in results)
            {
                KnowledgeGraphNode node = new KnowledgeGraphNode();
                node.Id = kvp.Key;
                node.Information = kvp.Value;

                nodeList.Add(node);
            }

            return nodeList;
        }
    }
}
