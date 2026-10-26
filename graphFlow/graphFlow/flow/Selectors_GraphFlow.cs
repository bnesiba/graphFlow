using ActionFlow.Models;
using GraphFlow.models;

namespace GraphFlow.flow
{
    //TODO: maybe selectors don't need to exist?

    public static class GraphStateSelectors<T>
    {
        public static FlowDataSelector<GraphOrchestrationState<T>, GraphRunState<T>> SelectByExecution(Guid executionId)
        {

            return new FlowDataSelector<GraphOrchestrationState<T>, GraphRunState<T>>((state) => state.GetValueOrDefault(executionId));
        }
    }
}
