using ActionFlow.Models;
using GraphFlow.models;

namespace GraphFlow.flow
{
    public class StateObjectReducer<T> : IFlowStateReducer<GraphFlowState<T>>
    {
        public GraphFlowState<T> InitialState => new GraphFlowState<T>();

        public List<IFlowReductionBase<GraphFlowState<T>>> Reductions => new List<IFlowReductionBase<GraphFlowState<T>>>
        {
            this.reduce(StateObject_OnNodeComplete_UpdateStateObject, Actions.NodeExecuted<T>()),
            this.reduce(StateObject_OnUpdateFlowState_UpdateStateObject, Actions.UpdateFlowState<T>())
        };

        //reducer methods
        public GraphFlowState<T> StateObject_OnUpdateFlowState_UpdateStateObject(FlowAction<StateUpdateRequest<T>> updateAction, GraphFlowState<T> currentState)
        {
            var updatedValue = updateAction.Parameters;
            Guid graphExecutionId = updatedValue.GraphExecutionId;
            currentState[graphExecutionId] = updatedValue.NewState;
            return currentState;
        }
        public GraphFlowState<T> StateObject_OnNodeComplete_UpdateStateObject(FlowAction<GraphNodeResult<T>> nodeExecutedAction, GraphFlowState<T> currentState)
        {
            var stateObj = nodeExecutedAction.Parameters.NodeOutput;
            Guid graphExecutionId = nodeExecutedAction.Parameters.GraphExecutionId;

            currentState[graphExecutionId] = stateObj;
            return currentState;
        }
    }

    public interface IDefaultValueHaver<T> where T : IDefaultValueHaver<T>
    {
        static abstract T DefaultValue();
    }
}
