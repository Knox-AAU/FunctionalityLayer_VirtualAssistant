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
        [Route("getNodes")]
        public IActionResult GetNodes(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Query parameter <name> may not be empty");
            }
            KnowledgeGraph graph = new KnowledgeGraph(new SparQLConnectionFactory());
            var nodes = graph.FindNodes(name);
            return new JsonResult(nodes);
        }

        [HttpGet]
        [Route("getNode")]
        //public KnowledgeGraphNode GetNode(string id, string type) //This is the correct version, but for the MVP we use the one below
        public IActionResult GetNode(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Query parameter <name> may not be empty");
            }
            KnowledgeGraph graph = new KnowledgeGraph(new SparQLConnectionFactory());
            var nodes = graph.FindNodes(name);
            var node = graph.FindNodeInformation(nodes.FirstOrDefault());
            return new JsonResult(node);
        }
    }
}
