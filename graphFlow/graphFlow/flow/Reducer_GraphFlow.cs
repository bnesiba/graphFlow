using ActionFlow.Models;
using graphFlow.models;
using GraphFlow.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphFlow.flow
{
    public class GraphRunStateReducer<T> : IFlowStateReducer<GraphRunState<T>>
    {
        public GraphRunState<T> InitialState => new GraphRunState<T>();

        public List<IFlowReductionBase<GraphRunState<T>>> Reductions => new List<IFlowReductionBase<GraphRunState<T>>>
        {
            this.reduce(GraphStateEvents_OnGraphExecution_AddGraphStart, Actions.GraphExecution<T>()),
            this.reduce(GraphStateEvents_OnGraphExecuted_AddGraphComplete, Actions.GraphExecuted<T>()),
            this.reduce(GraphStateEvents_OnNodeExecution_AddNodeStart, Actions.NodeExecution<T>()),
            this.reduce(GraphStateEvents_OnNodeExecuted_AddNodeComplete, Actions.NodeExecuted<T>()),
            this.reduce(GraphStateEvents_OnEdgeEvaluation_AddEdgeStart, Actions.EdgeEvaluation<T>()),
            this.reduce(GraphStateEvents_OnEdgeEvaluated_AddEdgeComplete, Actions.EdgeEvaluated<T>()),
            this.reduce(StateObject_OnUpdateFlowState_UpdateStateObject, Actions.UpdateFlowState<T>()),
        };

        public GraphRunState<T> GraphStateEvents_OnGraphExecution_AddGraphStart(FlowAction<ExecutableGraphRequest<T>> graphExecutionAction, GraphRunState<T> currentState)
        {
            var graphExecutionRequest = graphExecutionAction.Parameters;
            currentState.AddGraphStarted(graphExecutionRequest);
            return currentState;
        }

        public GraphRunState<T> GraphStateEvents_OnGraphExecuted_AddGraphComplete(FlowAction<ExecutableGraphResult<T>> graphExecutionAction, GraphRunState<T> currentState)
        {
            var graphExecutionResult = graphExecutionAction.Parameters;
            currentState.AddGraphComplete(graphExecutionResult);
            return currentState;
        }

        public GraphRunState<T> GraphStateEvents_OnNodeExecution_AddNodeStart(FlowAction<GraphNodeRequest<T>> nodeExecutedAction, GraphRunState<T> currentState)
        {
            var nodeExecutionRequest = nodeExecutedAction.Parameters;
            currentState.AddNodeStarted(nodeExecutionRequest);
            return currentState;
        }

        public GraphRunState<T> GraphStateEvents_OnNodeExecuted_AddNodeComplete(FlowAction<GraphNodeResult<T>> nodeExecutedAction, GraphRunState<T> currentState)
        {
            var nodeExecutionResult = nodeExecutedAction.Parameters;
            currentState.AddNodeComplete(nodeExecutionResult);
            return currentState;
        }

        public GraphRunState<T> GraphStateEvents_OnEdgeEvaluation_AddEdgeStart(FlowAction<GraphEdgeRequest<T>> edgeEvaluatedAction, GraphRunState<T> currentState)
        {
            var edgeEvaluatonRequest = edgeEvaluatedAction.Parameters;
            currentState.AddEdgeStarted(edgeEvaluatonRequest);
            return currentState;
        }

        public GraphRunState<T> GraphStateEvents_OnEdgeEvaluated_AddEdgeComplete(FlowAction<GraphEdgeResult<T>> edgeEvaluatedAction, GraphRunState<T> currentState)
        {
            var edgeEvaluationResult = edgeEvaluatedAction.Parameters;
            currentState.AddEdgeCompleted(edgeEvaluationResult);
            return currentState;
        }

        public GraphRunState<T> StateObject_OnUpdateFlowState_UpdateStateObject(FlowAction<T> updateAction, GraphRunState<T> currentState)
        {
            var updatedStateObj = updateAction.Parameters;
            if(updatedStateObj != null)
            {
                currentState.AddCheckpoint(updatedStateObj);

            }
            return currentState;
        }
    }
}
