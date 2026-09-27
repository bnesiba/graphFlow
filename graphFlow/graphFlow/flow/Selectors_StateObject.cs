using ActionFlow.Models;

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
}
