using ActionFlow;
using ExampleApi.Models;
using graphFlow.models;

namespace ExampleApi
{
    public class FlowGraph
    {
        private FlowState _flowState;
        private FlowStateData<GraphState<ExampleGraphStateObject>> _flowStateData;


        public FlowGraph(FlowState flowState, FlowStateData<GraphState<ExampleGraphStateObject>> flowStateData)
        {
            _flowState = flowState;
            _flowStateData = flowStateData;
        }
        public ExecutableGraph<ExampleGraphStateObject> GetFlowGraph()
        {
            ExecutableGraph<ExampleGraphStateObject> graph = new ExecutableGraph<ExampleGraphStateObject>(_flowState, _flowStateData);
            Executableno

        }

        public ExampleGraphStateObject Node1Function(ExampleGraphStateObject node) {

        }
    }
}
