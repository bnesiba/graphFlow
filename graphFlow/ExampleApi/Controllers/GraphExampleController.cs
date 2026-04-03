using ExampleApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExampleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraphExampleController : ControllerBase
    {
        private ExampleGraph _exampleGraph;

        public GraphExampleController(ExampleGraph exampleGraph)
        { 
            _exampleGraph = exampleGraph;
        }

        [HttpGet("test")]
        public virtual IActionResult RunTest()
        {
            var graph = _exampleGraph.GetGraph();
            var output = graph.ExecuteGraph();
            return StatusCode(200, output);
        }

        [HttpGet("testWithValue")]
        public virtual IActionResult RunTestWithValue()
        {
            var graph = _exampleGraph.GetGraph();
            var startingValue = ExampleGraphStateObject.DefaultValue();
            startingValue.ShouldDoTheThing = true;
            var output = graph.ExecuteGraph(startingValue);
            return StatusCode(200, output);
        }
    }

}
