using ActionFlow.Models;
using GraphFlow.models;

namespace GraphFlow.flow
{
    //TODO: update actionflow to make selectors unnecessary?

    public static class StateObjectSelectors<T> 
    {
        public static FlowDataSelector<GraphFlowState<T>, T> SelectByExecution(Guid executionId)
        {
            return new FlowDataSelector<GraphFlowState<T>, T>((state) => GetStateObject(state, executionId));
        }

        private static T GetStateObject(GraphFlowState<T> state, Guid graphExecutionId)
        {
            return state.GetValueOrDefault(graphExecutionId);
        }
    }
}
