using ActionFlow.Models;
using graphFlow.models;
using GraphFlow.models;

namespace GraphFlow.flow
{
    public class GraphFlowReducer<T> : IFlowStateReducer<GraphState<T>>
    {
        public GraphState<T> InitialState => new GraphState<T>();

        public List<IFlowReductionBase<GraphState<T>>> Reductions => new List<IFlowReductionBase<GraphState<T>>> 
        {
            this.reduce(GraphStateEvents_OnNodeExecution_AddNodeStart, Actions.NodeExecution<T>()),
            this.reduce(GraphStateEvents_OnNodeExecuted_AddNodeComplete, Actions.NodeExecuted<T>()),
            this.reduce(GraphStateEvents_OnEdgeEvaluated_AddEdgeEvaluation, Actions.EdgeEvaluated<T>()),
            this.reduce(StateObject_OnNodeComplete_UpdateStateObject, Actions.NodeExecuted<T>()),
            this.reduce(StateObject_OnUpdateFlowState_UpdateStateObject, Actions.UpdateFlowState<T>()),
        };

        //Reducer Methods
        public GraphState<T> GraphStateEvents_OnNodeExecution_AddNodeStart(FlowAction<GraphNode<T>> nodeExecutedAction, GraphState<T> currentState)
        {
            var result = nodeExecutedAction.Parameters;
            var graphEvent = new NodeStart<T>(currentState.stateObject, result.name, result.id, currentState.id);
            currentState.graphStateEvents.Add(graphEvent);
            return currentState;
        }
        public GraphState<T> GraphStateEvents_OnNodeExecuted_AddNodeComplete(FlowAction<GraphNodeResult<T>> nodeExecutedAction, GraphState<T> currentState)
        {
            var result = nodeExecutedAction.Parameters;
            var graphEvent = new NodeComplete<T>(result.nodeOutput, result.nodeExecuted.name, result.nodeExecuted.id, result.success, currentState.id);
            currentState.graphStateEvents.Add(graphEvent);
            return currentState;
        }
        public GraphState<T> GraphStateEvents_OnEdgeEvaluated_AddEdgeEvaluation(FlowAction<GraphEdgeResult<T>> edgeEvaluatedAction, GraphState<T> currentState)
        {
            var result = edgeEvaluatedAction.Parameters;
            var graphEvent = new EdgeEvaluation(result.edgeExecuted.targetNode.id, result.shouldContinue, currentState.id);
            currentState.graphStateEvents.Add(graphEvent);
            return currentState;
        }

        //TODO: consider removing stateobject from graphstate
        public GraphState<T> StateObject_OnUpdateFlowState_UpdateStateObject(FlowAction<T> updateAction, GraphState<T> currentState)
        {
            var updatedStateObj = updateAction.Parameters;
            currentState.stateObject = updatedStateObj;
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
