using ActionFlow.Models;
using GraphFlow.models;

namespace GraphFlow.flow
{
    //TODO: maybe selectors don't need to exist?
    public static class GraphFlowSelectors<T>
    {
        //public static FlowDataSelector<GraphState<T>, T> GetStateData = new FlowDataSelector<GraphState<T>, T>(GetStateObject);

        public static FlowDataSelector<GraphRunState<T>, GraphRunState<T>> GetGraphState = new FlowDataSelector<GraphRunState<T>, GraphRunState<T>>(GetState);

        private static T GetState<T>(T state)
        {
            return state;
        }

        //private static T  GetStateObject<T>(GraphState<T> graphState)
        //{
        //    return graphState.stateObject;
        //}
    }
}
