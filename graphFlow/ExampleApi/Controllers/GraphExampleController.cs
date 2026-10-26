using ExampleApi.Models;
using GraphFlow.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Text.Json;

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
            var output = graph.ExecuteGraph(ExampleGraphStateObject.DefaultValue());
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

        [HttpGet("testWithParallel")]
        public virtual IActionResult RunTestWithParallel()
        {
            var graph = _exampleGraph.GetGraph();
            
            List<ExecutableGraph<ExampleGraphStateObject>> graphs = new List<ExecutableGraph<ExampleGraphStateObject>> { graph, graph, graph };
            ConcurrentDictionary<Guid, ExampleGraphStateObject> resultValues = new ConcurrentDictionary<Guid, ExampleGraphStateObject>();
            ConcurrentDictionary<Guid, GraphRunState<ExampleGraphStateObject>> graphStates = new ConcurrentDictionary<Guid, GraphRunState<ExampleGraphStateObject>>();

            Parallel.ForEach(graphs, (graph) =>
            {
                var startingValue = ExampleGraphStateObject.DefaultValue();
                startingValue.ShouldDoTheThing = true;
                var output = graph.ExecuteGraph(startingValue);
                Guid arbitraryGuid = Guid.NewGuid();
                resultValues[arbitraryGuid] = output.StateObject;
                graphStates[arbitraryGuid] = output.GraphData;
            });

            return StatusCode(200, resultValues);
        }
    }

}
