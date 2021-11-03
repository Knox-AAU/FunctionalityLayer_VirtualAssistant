using NUnit.Framework;
using VirtualAssistantBusinessLogic.KnowledgeGraph;

namespace VirtualAssistantBusinessLogicTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void it_can_get_a_node_from_the_knowledge_graph_api()
        {
            KnowledgeGraph kg = new KnowledgeGraph();
            KnowledgeGraphNode node = kg.FindNode("Donald Trump");
            Assert.AreEqual("Donald Trump", node.Name);
        }
    }
}