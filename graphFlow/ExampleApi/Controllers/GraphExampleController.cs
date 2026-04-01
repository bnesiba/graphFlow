using ExampleApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExampleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraphExampleController : ControllerBase
    {
        private FlowGraph _flowGraph;

        public GraphExampleController(FlowGraph flowGraph)
        { 
            _flowGraph = flowGraph;
        }

        [HttpGet]
        public virtual IActionResult RunTest()
        {
            var graph = _flowGraph.GetFlowGraph();
            var output = graph.ExecuteGraph();
            return StatusCode(200, output);
        }
    }

}
