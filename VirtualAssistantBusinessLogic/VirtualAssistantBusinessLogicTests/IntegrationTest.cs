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
            Assert.AreEqual(19, results.Count);
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

        private ISparQLConnection GetWikidataSparQLConnectionMock()
        {
            XMLResponseDecoder responseDecoder = new();
            ISparQLConnection wikidataSparqlConnectionMock = Substitute.For<ISparQLConnection>();

            // Stream for all "Chris Evans"
            Stream allChrisEvansXmlStream = File.OpenRead("../../../TestFiles/ChrisEvans.xml");
            // Stream for former president Donald Trump
            Stream donaldTrumpXmlStream = File.OpenRead("../../../TestFiles/PresidentDonaldTrump.xml");

            // Use the xml streams instead of executing the queries
            wikidataSparqlConnectionMock
                .ExecuteQuery(Arg.Is<string>(x => x.Contains("\"Chris Evans\"")))
                .Returns(responseDecoder.Decode(allChrisEvansXmlStream));
            wikidataSparqlConnectionMock
                .ExecuteQuery(Arg.Is<string>(x => x.Contains(GetDonaldTrumpKnowledgeGraphNode().Id)))
                .Returns(responseDecoder.Decode(donaldTrumpXmlStream));

            // Supported types intersection
            wikidataSparqlConnectionMock
                .SupportedTypesIntersection(Arg.Is<List<string>>(x => x.Contains("human")))
                .Returns(new List<string> { "human" });
            wikidataSparqlConnectionMock
                .SupportedTypesIntersection(Arg.Is<List<string>>(x => x.Contains("")))
                .Returns(new List<string> { "" });

            // Sparql builder
            ISPOEncoder spoEncoder = new WikidataSPOEncoder();
            wikidataSparqlConnectionMock
                .GetSparQLBuilder(Arg.Is("human"))
                .Returns(new PersonSparQLBuilder(spoEncoder));
            wikidataSparqlConnectionMock
                .GetSparQLBuilder(Arg.Is(""))
                .Returns(new UnknownSparQLBuilder(spoEncoder));

            return wikidataSparqlConnectionMock;
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
