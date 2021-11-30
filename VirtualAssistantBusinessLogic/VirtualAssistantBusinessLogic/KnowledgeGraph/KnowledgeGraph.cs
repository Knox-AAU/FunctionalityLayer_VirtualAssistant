using System.Linq;
using VirtualAssistantBusinessLogic.SparQL;
using System.Collections.Generic;
using System;

namespace VirtualAssistantBusinessLogic.KnowledgeGraph
{
    /// <summary>
    /// Class representing the knowledge graph, that uses a SparQL Connection to
    /// find information from the knowledge graph
    /// </summary>
    public class KnowledgeGraph
    {
        public KnowledgeGraph(ISparQLConnectionFactory sparQLConnectionFactory)
        {
            SparQLConnectionFactory = sparQLConnectionFactory;
        }

        private ISparQLConnectionFactory SparQLConnectionFactory { get; set; }

        /// <summary>
        /// Takes a knowledge graph node and gets the relevant information for that node based on
        /// the nodes type.
        /// </summary>
        /// <param name="node">a specific knowledge graph node [returned from a call to find nodes]</param>
        /// <returns>The input node now containing information for the node</returns>
        public KnowledgeGraphNode FindNodeInformation(KnowledgeGraphNode node)
        {
            ValidateFindNodeInformationArguments(node);

            ISparQLConnection sparqlConnection = SparQLConnectionFactory.GetConnection();
            // Get the types from the node that are supported by the connection
            IEnumerable<string> supportedTypesInNode = sparqlConnection.SupportedTypesIntersection(node.Information["Type"]);
            // If the connection does not support any of the node's types return the node as it is
            if (!supportedTypesInNode.Any())
            {
                return node;
            }
            // Get the sparql builder from the connection
            ISparQLBuilder sparqlBuilder = sparqlConnection.GetSparQLBuilder(supportedTypesInNode.First());
            // Build the query
            sparqlBuilder.Query = node.Id;
            string sparqlQuery = sparqlBuilder.Build();
            // Execute the query
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
            if (string.IsNullOrWhiteSpace(query)) { throw new ArgumentException(); }

            ISparQLConnection sparqlConnection = SparQLConnectionFactory.GetConnection();
            ISparQLBuilder sparqlBuilder = sparqlConnection.GetSparQLBuilder("");
            sparqlBuilder.Query = query;
            string sparqlQuery = sparqlBuilder.Build();
            Dictionary<string, Dictionary<string, List<string>>> results = sparqlConnection.ExecuteQuery(sparqlQuery);

            //Create the node list that is returned
            List<KnowledgeGraphNode> nodeList = new();
            //Go through each key-value pair (kvp) from the executed query and create and append the nodes to the node list
            foreach (var kvp in results)
            {
                KnowledgeGraphNode node = new()
                {
                    Id = kvp.Key,
                    Information = kvp.Value,
                    Name = kvp.Value.ContainsKey("birth name") ? kvp.Value["birth name"].FirstOrDefault() : query,
                };

                nodeList.Add(node);
            }

            return nodeList;
        }

        /// <summary>
        /// Validates the arguments for the FindNodeInformation method
        /// </summary>
        /// <param name="node">Must not be empty. Must contain a type key in the information. Must have a set ID.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException">If not containing needed information</exception>
        private void ValidateFindNodeInformationArguments(KnowledgeGraphNode node)
        {
            if (node == null) { throw new ArgumentNullException(); }
            if (!node.Information.ContainsKey("Type")) { throw new ArgumentException("Node information must contain type to find more information about the node"); }
            if (string.IsNullOrWhiteSpace(node.Id) || node.Id == "_") { throw new ArgumentException("Node ID must be set to find more information about the node"); }
        }
    }
}
