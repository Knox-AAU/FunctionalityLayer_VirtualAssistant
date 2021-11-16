using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualAssistantBusinessLogic.KnowledgeGraph;
using VirtualAssistantBusinessLogic.SparQL;
namespace VirtualAssistantAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KnowledgeGraphController : ControllerBase
    {
        private readonly ILogger<KnowledgeGraphController> _logger;

        public KnowledgeGraphController(ILogger<KnowledgeGraphController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("[controller]/getNodes/{name}")]
        public List<KnowledgeGraphNode> GetNodes(string name)
        {
            KnowledgeGraph graph = new KnowledgeGraph(new SparQLConnectionFactory());
            var nodes = graph.FindNodes(name);
            return nodes;
        }

        [HttpGet]
        [Route("[controller]/getNode/{name}")]
        public KnowledgeGraphNode GetNode(string name)
        {
            KnowledgeGraph graph = new KnowledgeGraph(new SparQLConnectionFactory());
            var nodes = graph.FindNodes(name);
            var node = graph.FindNodeInformation(nodes.FirstOrDefault());
            return node;
        }
    }
}
