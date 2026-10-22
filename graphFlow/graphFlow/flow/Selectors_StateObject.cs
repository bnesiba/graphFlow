using ActionFlow.Models;
using GraphFlow.models;

namespace GraphFlow.flow
{
    //TODO: update actionflow to make selectors unnecessary?
    public static class StateObjectSelectors<T>
    {
        public static FlowDataSelector<T, T> GetStateData = new FlowDataSelector<T, T>(GetState);


        private static T GetState<T>(T state)
        {
            return state;
        }
    }


    public static class GraphStateSelectors<T>
    {
        public static FlowDataSelector<GraphOrchestrationState<T>, GraphRunState<T>> SelectByExecution(Guid executionId)
        {

            return new FlowDataSelector<GraphOrchestrationState<T>, GraphRunState<T>>( (state) => state.GetValueOrDefault(executionId));
        }
    }    
}
