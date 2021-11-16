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
        public void it_can_get_denmark_nodes_and_find_information_for_a_specific_denmark_from_the_knowledge_graph_api()
        {
            KnowledgeGraph kg = new KnowledgeGraph(new SparQLConnectionFactory());
            List<KnowledgeGraphNode> results = kg.FindNodes("Denmark");
            int index = -1;
            for(int i = 0; i < results.Count; ++i)
            {
                KnowledgeGraphNode kgn = results[i];
                if (kgn.Id == "wd:Q35")
                {
                    index = i;
                    break;
                }
            }
            Assert.IsTrue(index != -1, "The expected Denmark was not in the result list.");
            KnowledgeGraphNode denmarkNode = kg.FindNodeInformation(results[index]);
            Assert.AreEqual("Copenhagen", denmarkNode.Information["Capital"][0]);
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

        private KnowledgeGraphNode GetDenmarkKnowledgeGraphNode()
        {
            KnowledgeGraphNode node = new KnowledgeGraphNode();
            node.Id = "wd:Q35";
            node.Name = "Denmark";
            node.Information = new Dictionary<string, List<string>>();
            node.Information["Type"] = new List<string>();
            node.Information["Type"].Add("country");
            node.Information["Type"].Add("state");
            node.Information["Type"].Add("sovereign state");
            node.Information["Type"].Add("colonial power");
            node.Information["Type"].Add("country bordering the Baltic Sea");
            node.Information["Type"].Add("autonomous country within the Kingdom of Denmark");
            return node;
        }
    }
}
