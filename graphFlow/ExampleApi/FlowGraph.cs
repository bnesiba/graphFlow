using ActionFlow;
using ExampleApi.Models;
using graphFlow.models;

namespace ExampleApi
{
    public class FlowGraph
    {
        private FlowState _flowState;
        private FlowStateData<ExampleGraphStateObject> _flowStateData;


        public FlowGraph(FlowState flowState, FlowStateData<ExampleGraphStateObject> flowStateData)
        {
            _flowState = flowState;
            _flowStateData = flowStateData;
        }
        public ExecutableGraph<ExampleGraphStateObject> GetFlowGraph()
        {
            ExecutableGraph<ExampleGraphStateObject> graph = new ExecutableGraph<ExampleGraphStateObject>(_flowState, _flowStateData);
            graph.AddNode("Node1", Node1Function);
            graph.AddNode("Node2", Node2Function);
            graph.AddNode("Node3", Node3Function);
            graph.AddNode("Node4", Node4Function);

            graph.AddEdge("Node1", "Node2");
            graph.AddEdge("Node2", "Node3", ShouldNotDoTheThing);
            graph.AddEdge("Node2", "Node4", ShouldWeDoTheThing);

            graph.SetStartNode("Node1");
            return graph;
        }

        //Nodes
        public ExampleGraphStateObject Node1Function(ExampleGraphStateObject injectedState) 
        {
            injectedState.NodeOutputs.TryGetValue("Node1", out string? node1Value);
            node1Value = node1Value ?? string.Empty;
            injectedState.NodeOutputs["Node1"] = node1Value + "Node1Run!";
            injectedState.ShouldDoTheThing = true;
            injectedState.NodeCount += 1;
            return injectedState;
        }

        public ExampleGraphStateObject Node2Function(ExampleGraphStateObject injectedState) 
        {
            if (injectedState.ShouldDoTheThing)
            {
                //injectedState.NodeOutputs.Add("Node2", "Node2DidTheThing!");
                injectedState.NodeOutputs.TryGetValue("Node2", out string? node2Value);
                node2Value = node2Value ?? string.Empty;
                injectedState.NodeOutputs["Node2"] = node2Value + "Node2DidTheThing!";
            }
            else
            {
                //injectedState.NodeOutputs.Add("Node2", "Node2DidNotDoTheThing!");
                injectedState.NodeOutputs.TryGetValue("Node2", out string? node2Value);
                node2Value = node2Value ?? string.Empty;
                injectedState.NodeOutputs["Node2"] = node2Value + "Node2DidNotDoTheThing!";
            }
            injectedState.NodeCount += 1;
            return injectedState;
        }

        public ExampleGraphStateObject Node3Function(ExampleGraphStateObject injectedState)
        {
            //injectedState.NodeOutputs.Add("Node3", "MadeItToNode3");
            injectedState.NodeOutputs.TryGetValue("Node3", out string? node3Value);
            node3Value = node3Value ?? string.Empty;
            injectedState.NodeOutputs["Node3"] = node3Value + "MadeItToNode3";
            injectedState.NodeCount += 1;
            return injectedState;
        }

        public ExampleGraphStateObject Node4Function(ExampleGraphStateObject injectedState)
        {
            //injectedState.NodeOutputs.Add("Node4", "Node4Reached!");
            injectedState.NodeOutputs.TryGetValue("Node4", out string? node4Value);
            node4Value = node4Value ?? string.Empty;
            injectedState.NodeOutputs["Node4"] = node4Value + "Node4Reached!";
            injectedState.NodeCount += 1;
            return injectedState;
        }

        //edges
        public bool ShouldWeDoTheThing(ExampleGraphStateObject injectedState)
        {
            return injectedState.ShouldDoTheThing;
        }

        public bool ShouldNotDoTheThing(ExampleGraphStateObject injectedState)
        {
            return !injectedState.ShouldDoTheThing;
        }
    }
}
