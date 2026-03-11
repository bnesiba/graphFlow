using ActionFlow.Models;
using graphFlow.models;

namespace graphFlow
{
    public class Reducer<T> : IFlowStateReducer<GraphState<T>>
    {
        public GraphState<T> InitialState => new GraphState<T>();

        public List<IFlowReductionBase<GraphState<T>>> Reductions => new List<IFlowReductionBase<GraphState<T>>> 
        {
            this.reduce(nodesAndEdgesRun_OnNodeComplete_AddNodesAndEdgesRun, Actions.NodeExecuted<T>()),
            this.reduce(nodesAndEdgesRun_OnEdgeEvaluation_AddNodesAndEdgesRun, Actions.EdgeEvaluation<T>()),
            this.reduce(StateObject_OnNodeComplete_UpdateStateObject, Actions.NodeExecuted<T>())
        };

        //Reducer Methods
        public GraphState<T> nodesAndEdgesRun_OnNodeComplete_AddNodesAndEdgesRun(FlowAction<GraphNodeResult<T>> nodeExecutedAction, GraphState<T> currentState)
        {
            currentState.nodesAndEdgesRun.Add(nodeExecutedAction.Parameters.nodeExecuted.name);
            return currentState;
        }
        public GraphState<T> nodesAndEdgesRun_OnEdgeEvaluation_AddNodesAndEdgesRun(FlowAction<GraphEdge<T>> edgeEvaluatedAction, GraphState<T> currentState)
        {
            currentState.nodesAndEdgesRun.Add(edgeEvaluatedAction.Parameters.name);
            return currentState;
        }

        //TODO: implement properly. This will blow up on partials, which we don't want.
        public GraphState<T> StateObject_OnNodeComplete_UpdateStateObject(FlowAction<GraphNodeResult<T>> nodeExecutedAction, GraphState<T> currentState)
        {
            var stateObj = nodeExecutedAction.Parameters.nodeOutput;
            currentState.stateObject = stateObj;
            return currentState;
        }
    }
}
