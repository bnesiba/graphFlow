using ExampleApi.Models;
using graphFlow.models;
using graphFlow.util;

namespace ExampleApi
{
    //        [Node1]
    //           |
    //           v
    //        [Node2]
    //         /    \
    //        /      \
    //   if(ShouldDoTheThing)
    //    false        true
    //      v            v
    //   [Node3]      [Node4]

    //Final State:
    // (ExampleGraphState)
    //{
    //    "nodeOutputs": {
    //        "Node1": "Node1Run",
    //        "Node2": "Node2DidTheThing",
    //        "Node4": "Node4Reached"
    //    },
    //    "nodeCount": 3,
    //    "shouldDoTheThing": true
    //}

    public class ExampleGraph
    {
        private GraphBuilder<ExampleGraphStateObject> _graphBuilder;

        public ExampleGraph(GraphBuilder<ExampleGraphStateObject> graphBuilder)
        {
            _graphBuilder = graphBuilder;
        }

        //Define Graph
        public ExecutableGraph<ExampleGraphStateObject> GetGraph()
        {
            ExecutableGraph<ExampleGraphStateObject> graph = _graphBuilder.GetExecutableGraph();
            graph.AddNode("Node1", Node1Function);
            graph.AddNode("Node2", Node2Function);
            graph.AddNode("Node3", Node3Function);
            graph.AddNode("Node4", Node4Function);

            graph.AddEdge("Node1", "Node2");
            graph.AddEdge("Node2", "Node3", ShouldNotDoTheThing);
            graph.AddEdge("Node2", "Node4", ShouldDoTheThing);

            graph.SetStartNode("Node1");
            return graph;
        }

        //Node Functions
        public ExampleGraphStateObject Node1Function(ExampleGraphStateObject injectedState) 
        {
            injectedState.NodeOutputs.TryGetValue("Node1", out string? node1Value);
            injectedState.NodeOutputs["Node1"] = node1Value + "Node1Run";
            injectedState.NodeCount++;
            return injectedState;
        }

        public ExampleGraphStateObject Node2Function(ExampleGraphStateObject injectedState) 
        {
            if (injectedState.ShouldDoTheThing)
            {
                injectedState.NodeOutputs.TryGetValue("Node2", out string? node2Value);
                injectedState.NodeOutputs["Node2"] = node2Value + "Node2DidTheThing";
            }
            else
            {
                injectedState.NodeOutputs.TryGetValue("Node2", out string? node2Value);
                injectedState.NodeOutputs["Node2"] = node2Value + "Node2DidNotDoTheThing";
            }
            injectedState.NodeCount++;
            return injectedState;
        }

        public ExampleGraphStateObject Node3Function(ExampleGraphStateObject injectedState)
        {
            injectedState.NodeOutputs.TryGetValue("Node3", out string? node3Value);
            injectedState.NodeOutputs["Node3"] = node3Value + "MadeItToNode3";
            injectedState.NodeCount++;
            return injectedState;
        }

        public ExampleGraphStateObject Node4Function(ExampleGraphStateObject injectedState)
        {
            injectedState.NodeOutputs.TryGetValue("Node4", out string? node4Value);
            injectedState.NodeOutputs["Node4"] = node4Value + "Node4Reached";
            injectedState.NodeCount++;
            return injectedState;
        }

        //Edge Functions
        public bool ShouldDoTheThing(ExampleGraphStateObject injectedState)
        {
            return injectedState.ShouldDoTheThing;
        }

        public bool ShouldNotDoTheThing(ExampleGraphStateObject injectedState)
        {
            return !injectedState.ShouldDoTheThing;
        }
    }
}
