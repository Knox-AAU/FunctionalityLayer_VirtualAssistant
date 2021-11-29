using NSubstitute;
using NUnit.Framework;
using System.IO;
using System.Collections.Generic;
using VirtualAssistantBusinessLogic.KnowledgeGraph;
using VirtualAssistantBusinessLogic.SparQL;
using System;

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
            KnowledgeGraph kg = GetKnowledgeGraph();
            List<KnowledgeGraphNode> results = kg.FindNodes("Chris Evans");
            Assert.AreEqual(21, results.Count);
            //TODO more asserts
        }

        [Test]
        public void it_can_get_information_for_a_specific_node()
        {
            KnowledgeGraph kg = GetKnowledgeGraph();
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

        private ISparQLConnection GetWikidataSparQLConnectionMock()
        {
            XMLResponseDecoder responseDecoder = new();
            ISparQLConnection wikidataSparqlMock = Substitute.For<WikidataSparQLConnection>(responseDecoder);

            Stream xmlStream = File.OpenRead("../../../TestFiles/PresidentDonaldTrump.xml");

            wikidataSparqlMock.ExecuteQuery(Arg.Is<string>(x => ContainsAnIdentifyingVariableInQuery(x)).Returns(responseDecoder.Decode(xmlStream));

            return wikidataSparqlMock;
        }

        private KnowledgeGraph GetKnowledgeGraph()
        {
            var stub = Substitute.For<ISparQLConnectionFactory>();
            stub.GetConnection().Returns(_ => GetWikidataSparQLConnectionMock());
            return new KnowledgeGraph(stub);
        }

        public static bool ContainsAnIdentifyingVariableInQuery(string query)
        {
            return query.Contains("?s0");
        }
    }
}
