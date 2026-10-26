using ActionFlow.Models;
using GraphFlow.models;

namespace GraphFlow.flow
{
    //TODO: update ActionFlow to make selectors less obtuse or unnecessary
    public static class GraphStreamSelectors<T>
    {
        public static FlowDataSelector<GraphStreamState<T>, ExecutableGraph<T>> SelectByExecution(Guid executionId)
        {
            return new FlowDataSelector<GraphStreamState<T>, ExecutableGraph<T>>((state) => state.GetValueOrDefault(executionId));
        }
    }
}
