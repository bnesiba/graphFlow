using ActionFlow.Models;
using GraphFlow.models;

namespace GraphFlow.flow
{
    public class GraphRunStateReducer<T> : IFlowStateReducer<GraphOrchestrationState<T>>
    {
        public GraphOrchestrationState<T> InitialState => new GraphOrchestrationState<T>(); //TODO: populate?

        public List<IFlowReductionBase<GraphOrchestrationState<T>>> Reductions => new List<IFlowReductionBase<GraphOrchestrationState<T>>>
        {
            this.reduce(GraphStateEvents_OnGraphExecution_AddGraphStart, Actions.GraphExecution<T>()),
            this.reduce(GraphStateEvents_OnGraphExecuted_AddGraphComplete, Actions.GraphExecuted<T>()),
            this.reduce(GraphStateEvents_OnNodeExecution_AddNodeStart, Actions.NodeExecution<T>()),
            this.reduce(GraphStateEvents_OnNodeExecuted_AddNodeComplete, Actions.NodeExecuted<T>()),
            this.reduce(GraphStateEvents_OnEdgeEvaluation_AddEdgeStart, Actions.EdgeEvaluation<T>()),
            this.reduce(GraphStateEvents_OnEdgeEvaluated_AddEdgeComplete, Actions.EdgeEvaluated<T>()),
            this.reduce(StateObject_OnUpdateFlowState_UpdateStateObject, Actions.UpdateFlowState<T>()),
        };

        public GraphOrchestrationState<T> GraphStateEvents_OnGraphExecution_AddGraphStart(FlowAction<ExecutableGraphRequest<T>> graphExecutionAction, GraphOrchestrationState<T> currentState)
        {
            var graphExecutionRequest = graphExecutionAction.Parameters;
            currentState.AddGraphStarted(graphExecutionRequest);
            return currentState;
        }

        public GraphOrchestrationState<T> GraphStateEvents_OnGraphExecuted_AddGraphComplete(FlowAction<ExecutableGraphResult<T>> graphExecutionAction, GraphOrchestrationState<T> currentState)
        {
            var graphExecutionResult = graphExecutionAction.Parameters;
            currentState.AddGraphComplete(graphExecutionResult);
            return currentState;
        }

        public GraphOrchestrationState<T> GraphStateEvents_OnNodeExecution_AddNodeStart(FlowAction<GraphNodeRequest<T>> nodeExecutedAction, GraphOrchestrationState<T> currentState)
        {
            var nodeExecutionRequest = nodeExecutedAction.Parameters;
            currentState.AddNodeStarted(nodeExecutionRequest);
            return currentState;
        }

        public GraphOrchestrationState<T> GraphStateEvents_OnNodeExecuted_AddNodeComplete(FlowAction<GraphNodeResult<T>> nodeExecutedAction, GraphOrchestrationState<T> currentState)
        {
            var nodeExecutionResult = nodeExecutedAction.Parameters;
            currentState.AddNodeComplete(nodeExecutionResult);
            return currentState;
        }

        public GraphOrchestrationState<T> GraphStateEvents_OnEdgeEvaluation_AddEdgeStart(FlowAction<GraphEdgeRequest<T>> edgeEvaluatedAction, GraphOrchestrationState<T> currentState)
        {
            var edgeEvaluatonRequest = edgeEvaluatedAction.Parameters;
            currentState.AddEdgeStarted(edgeEvaluatonRequest);
            return currentState;
        }

        public GraphOrchestrationState<T> GraphStateEvents_OnEdgeEvaluated_AddEdgeComplete(FlowAction<GraphEdgeResult<T>> edgeEvaluatedAction, GraphOrchestrationState<T> currentState)
        {
            var edgeEvaluationResult = edgeEvaluatedAction.Parameters;
            currentState.AddEdgeCompleted(edgeEvaluationResult);
            return currentState;
        }

        public GraphOrchestrationState<T> StateObject_OnUpdateFlowState_UpdateStateObject(FlowAction<T> updateAction, GraphOrchestrationState<T> currentState)
        {
            var updatedStateObj = updateAction.Parameters;
            if(updatedStateObj != null)
            {
                currentState.AddGraphUpdate(updatedStateObj);

            }
            return currentState;
        }
    }
}
