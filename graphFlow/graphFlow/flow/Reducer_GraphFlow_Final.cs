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
    public class FinalGraphFlowReducer<T> : IFlowStateReducer<GraphRunState<T>>
    {
        public GraphRunState<T> InitialState => new GraphRunState<T>();

        public List<IFlowReductionBase<GraphRunState<T>>> Reductions => new List<IFlowReductionBase<GraphRunState<T>>>
        {
            this.reduce(GraphStateEvents_OnGraphExecution_AddGraphStart, Actions.GraphExecution<T>()),
            this.reduce(GraphStateEvents_OnGraphExecuted_AddGraphComplete, Actions.GraphExecuted<T>()),
            this.reduce(GraphStateEvents_OnNodeExecution_AddNodeStart, Actions.NodeExecution<T>()),
            this.reduce(GraphStateEvents_OnNodeExecuted_AddNodeComplete, Actions.NodeExecuted<T>()),
            this.reduce(GraphStateEvents_OnEdgeEvaluated_AddEdgeEvaluation, Actions.EdgeEvaluated<T>()),
            this.reduce(StateObject_OnNodeComplete_UpdateStateObject, Actions.NodeExecuted<T>()),
            this.reduce(StateObject_OnUpdateFlowState_UpdateStateObject, Actions.UpdateFlowState<T>()),
        };

        //reducer methods TODO: start here <- !
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

        public GraphRunState<T> GraphStateEvents_OnNodeExecution_AddNodeStart(FlowAction<GraphNode<T>> nodeExecutedAction, GraphRunState<T> currentState)
        {

            return currentState;
        }
        public GraphRunState<T> GraphStateEvents_OnNodeExecuted_AddNodeComplete(FlowAction<GraphNodeResult<T>> nodeExecutedAction, GraphRunState<T> currentState)
        {

            return currentState;
        }
        public GraphRunState<T> GraphStateEvents_OnEdgeEvaluated_AddEdgeEvaluation(FlowAction<GraphEdgeResult<T>> edgeEvaluatedAction, GraphRunState<T> currentState)
        {

            return currentState;
        }

        //TODO: consider removing stateobject from graphstate
        public GraphRunState<T> StateObject_OnUpdateFlowState_UpdateStateObject(FlowAction<T> updateAction, GraphRunState<T> currentState)
        {
            var updatedStateObj = updateAction.Parameters;
            currentState.stateObject = updatedStateObj;
            return currentState;
        }

        //TODO: remove or implement properly. This will blow up on partials, which we don't want.
        public GraphRunState<T> StateObject_OnNodeComplete_UpdateStateObject(FlowAction<GraphNodeResult<T>> nodeExecutedAction, GraphRunState<T> currentState)
        {
            var stateObj = nodeExecutedAction.Parameters.NodeOutput;
            currentState.stateObject = stateObj;
            return currentState;
        }
    }
}
