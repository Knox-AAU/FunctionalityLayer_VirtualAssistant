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
        [ProducesResponseType(typeof(KnowledgeGraphNode), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult GetNodes(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Query parameter <name> may not be empty");
            }
            KnowledgeGraph graph = new KnowledgeGraph(new SparQLConnectionFactory());
            var nodes = graph.FindNodes(name);
            if (nodes == null || nodes.Count() == 0)
            {
                return NoContent();
            }
            return new JsonResult(nodes);
        }

        [HttpGet]
        [Route("getNode")]
        [ProducesResponseType(typeof(KnowledgeGraphNode), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        //public KnowledgeGraphNode GetNode(string id, string type) //This is the correct version, but for the MVP we use the one below
        public IActionResult GetNode(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Query parameter <name> may not be empty");
            }
            KnowledgeGraph graph = new KnowledgeGraph(new SparQLConnectionFactory());
            var nodes = graph.FindNodes(name);
            if (nodes == null || nodes.Count() == 0)
            {
                return NoContent();
            }
            var node = graph.FindNodeInformation(nodes.First());
            return new JsonResult(node);
        }
    }
}
