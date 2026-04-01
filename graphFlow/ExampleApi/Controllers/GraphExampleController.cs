using ExampleApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExampleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraphExampleController : ControllerBase
    {
        private ExampleGraph _flowGraph;

        public GraphExampleController(ExampleGraph flowGraph)
        { 
            _flowGraph = flowGraph;
        }

        [HttpGet]
        public virtual IActionResult RunTest()
        {
            var graph = _flowGraph.GetGraph();
            var output = graph.ExecuteGraph();
            return StatusCode(200, output);
        }
    }

}
