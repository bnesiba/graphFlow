using ActionFlow;
using ActionFlow.Models;
using graphFlow.models;

namespace GraphFlow.flow
{
    public class StateObjectReducer<T> : IFlowStateReducer<T> where T : IDefaultValueHaver<T>
    {
        public T InitialState => T.DefaultValue();

        public List<IFlowReductionBase<T>> Reductions => new List<IFlowReductionBase<T>>
        {
            this.reduce(StateObject_OnNodeComplete_UpdateStateObject, Actions.NodeExecuted<T>()),
            this.reduce(StateObject_OnUpdateFlowState_UpdateStateObject, Actions.UpdateFlowState<T>())
        };

        //reducer methods
        public T StateObject_OnUpdateFlowState_UpdateStateObject(FlowAction<T> updateAction, T currentState)
        {
            var updatedValue = updateAction.Parameters;
            currentState = updatedValue;
            return currentState;
        }
        public T StateObject_OnNodeComplete_UpdateStateObject(FlowAction<GraphNodeResult<T>> nodeExecutedAction, T currentState)
        {
            var stateObj = nodeExecutedAction.Parameters.nodeOutput;
            currentState = stateObj;
            return currentState;
        }
    }

    public interface IDefaultValueHaver<T> where T : IDefaultValueHaver<T>
    {
        static abstract T DefaultValue();
    }
}
