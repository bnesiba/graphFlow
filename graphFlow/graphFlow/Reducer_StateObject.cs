using ActionFlow;
using ActionFlow.Models;
using graphFlow.models;

namespace graphFlow
{
    public class StateObjectReducer<T> : IFlowStateReducer<T> where T : IDefaultValueHaver<T>
    {
        public T InitialState => T.DefaultValue();

        public List<IFlowReductionBase<T>> Reductions => new List<IFlowReductionBase<T>>
        {
            this.reduce(StateObject_OnNodeComplete_UpdateStateObject, Actions.NodeExecuted<T>())
        };

        //reducer methods

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
