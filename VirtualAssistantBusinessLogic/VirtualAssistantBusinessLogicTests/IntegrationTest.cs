using NUnit.Framework;
using System.Collections.Generic;
using VirtualAssistantBusinessLogic.KnowledgeGraph;
using VirtualAssistantBusinessLogic.SparQL;

namespace VirtualAssistantBusinessLogicTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void it_can_get_nodes_from_the_knowledge_graph_api()
        {
            KnowledgeGraph kg = new KnowledgeGraph(new SparQLConnectionFactory());
            List<KnowledgeGraphNode> results = kg.FindNodes("Chris Evans");
            Assert.AreEqual(21, results.Count);
            //TODO more asserts
        }

        [Test]
        public void it_can_get_information_for_a_specific_node()
        {
            KnowledgeGraph kg = new KnowledgeGraph(new SparQLConnectionFactory());
            KnowledgeGraphNode result = kg.FindNodeInformation(GetDonaldTrumpKnowledgeGraphNode());
            Assert.IsTrue(result.Information.ContainsKey("Spouse"));
            Assert.AreEqual("Melania Trump", result.Information["Spouse"][0]);
        }

        private KnowledgeGraphNode GetDonaldTrumpKnowledgeGraphNode()
        {
            KnowledgeGraphNode node = new KnowledgeGraphNode();
            node.Id = "wd:Q22686";
            node.Name = "Donald Trump";
            node.Information = new Dictionary<string, List<string>>();
            node.Information["Type"] = new List<string>();
            node.Information["Type"].Add("human");
            return node;
        }
    }
}
