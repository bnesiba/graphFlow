using ActionFlow;
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
    public class GraphStateReducer<T> : IFlowStateReducer<GraphRunState<T>>
    {
        private FlowStateData<T> _flowStateData;
        private Dictionary<Guid, Stack<Guid>> _incompleteNodeRuns;
        private Dictionary<Guid, Stack<Guid>> _incompleteEdgeRuns;
        public GraphRunState<T> InitialState => new GraphRunState<T>();

        public GraphStateReducer(FlowStateData<T> flowStateData)
        {
            _flowStateData = flowStateData;
            _incompleteNodeRuns = new Dictionary<Guid, Stack<Guid>>();
            _incompleteEdgeRuns = new Dictionary<Guid, Stack<Guid>>();
        }


        public List<IFlowReductionBase<GraphRunState<T>>> Reductions => new List<IFlowReductionBase<GraphRunState<T>>>
        {
            this.reduce(GraphStateEvents_OnNodeExecution_AddNodeStart, Actions.NodeExecution<T>()),
            this.reduce(GraphStateEvents_OnNodeExecuted_AddNodeComplete, Actions.NodeExecuted<T>()),
            this.reduce(GraphStateEvents_OnEdgeEvaluated_AddEdgeEvaluation, Actions.EdgeEvaluated<T>()),
            this.reduce(StateObject_OnNodeComplete_UpdateStateObject, Actions.NodeExecuted<T>()),
            this.reduce(StateObject_OnUpdateFlowState_UpdateStateObject, Actions.UpdateFlowState<T>()),
        };

        //Reducer Methods
        public GraphRunState<T> GraphStateEvents_OnNodeExecution_AddNodeStart(FlowAction<GraphNode<T>> nodeExecutedAction, GraphRunState<T> currentState)
        {
            var result = nodeExecutedAction.Parameters;
            currentState.AddNodeStarted(result);
            var graphEvent = new NodeStart<T>(currentState.stateObject, result.name, result.id, currentState.id);
            currentState.graphStateEvents.Add(graphEvent);
            return currentState;
        }
        public GraphRunState<T> GraphStateEvents_OnNodeExecuted_AddNodeComplete(FlowAction<GraphNodeResult<T>> nodeExecutedAction, GraphRunState<T> currentState)
        {
            var result = nodeExecutedAction.Parameters;
            var graphEvent = new NodeComplete<T>(result.NodeOutput, result.NodeExecuted.name, result.NodeExecuted.id, result.Success, currentState.id);
            currentState.graphStateEvents.Add(graphEvent);
            return currentState;
        }
        public GraphRunState<T> GraphStateEvents_OnEdgeEvaluated_AddEdgeEvaluation(FlowAction<GraphEdgeResult<T>> edgeEvaluatedAction, GraphRunState<T> currentState)
        {
            var result = edgeEvaluatedAction.Parameters;
            var graphEvent = new EdgeEvaluation(result.edgeExecuted.targetNode.id, result.shouldContinue, currentState.id);
            currentState.graphStateEvents.Add(graphEvent);
            return currentState;
        }

        //TODO: consider removing stateobject from graphstate
        public GraphRunState<T> StateObject_OnUpdateFlowState_UpdateStateObject(FlowAction<T> updateAction, GraphRunState<T> currentState)
        {
            var updatedStateObj = updateAction.Parameters;
            currentState.stateObject = updatedStateObj;
            return currentState;
        }

        //TODO: implement properly. This will blow up on partials, which we don't want.
        public GraphRunState<T> StateObject_OnNodeComplete_UpdateStateObject(FlowAction<GraphNodeResult<T>> nodeExecutedAction, GraphRunState<T> currentState)
        {
            var stateObj = nodeExecutedAction.Parameters.NodeOutput;
            currentState.stateObject = stateObj;
            return currentState;
        }
    }
}
