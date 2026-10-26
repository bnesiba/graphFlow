using ActionFlow.Models;
using GraphFlow.models;

namespace GraphFlow.flow
{
    public class GraphStreamReducer<T> : IFlowStateReducer<GraphStreamState<T>>
    {
        public GraphStreamState<T> InitialState => new GraphStreamState<T>();

        public List<IFlowReductionBase<GraphStreamState<T>>> Reductions => new List<IFlowReductionBase<GraphStreamState<T>>>
        {
            this.reduce(GraphsStreaming_OnGraphExecution_AddGraph, Actions.GraphExecution<T>()),
            this.reduce(GraphsStreaming_OnGraphExecuted_RemoveGraph, Actions.GraphExecuted<T>())
        };

        public GraphStreamState<T> GraphsStreaming_OnGraphExecution_AddGraph(FlowAction<ExecutableGraphRequest<T>> graphExecutionAction, GraphStreamState<T> currentState)
        {
            Guid graphExecutionId = graphExecutionAction.Parameters.GraphExecutionId;
            ExecutableGraph<T> executableGraph = graphExecutionAction.Parameters.ExecutingGraph;
            currentState[graphExecutionId] = executableGraph;
            return currentState;
        }

        public GraphStreamState<T> GraphsStreaming_OnGraphExecuted_RemoveGraph(FlowAction<ExecutableGraphResult<T>> graphExecutionAction, GraphStreamState<T> currentState)
        {
            Guid graphExecutionId = graphExecutionAction.Parameters.GraphExecutionId;
            currentState.Remove(graphExecutionId, out _);
            return currentState;
        }

    }
}
