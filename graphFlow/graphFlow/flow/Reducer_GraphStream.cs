using ActionFlow.Models;
using GraphFlow.models;
namespace GraphFlow.flow
{
    public class GraphStreamReducer<T> : IFlowStateReducer<GraphStreamState<T>>
    {
        public GraphStreamState<T> InitialState => new GraphStreamState<T>();

        public List<IFlowReductionBase<GraphStreamState<T>>> Reductions => new List<IFlowReductionBase<GraphStreamState<T>>>
        {
            this.reduce(GraphStateEvents_OnGraphExecution_AddToRunningGraphs, Actions.GraphExecution<T>()),
            this.reduce(GraphStateEvents_OnGraphExecuted_RemoveFromRunningGraphs, Actions.GraphExecuted<T>()),
        };

        //reducer functions
        
        public GraphStreamState<T> GraphStateEvents_OnGraphExecution_AddToRunningGraphs(FlowAction<ExecutableGraphRequest<T>> graphExecutionAction, GraphStreamState<T> currentState)
        {
            var graphExecutionRequest = graphExecutionAction.Parameters;
            var GraphExecutionId = graphExecutionAction.Parameters.GraphExecutionId;
            var ExecutingGraph = graphExecutionAction.Parameters.ExecutingGraph;
            var existingGraph = currentState.runningGraphs.GetValueOrDefault(GraphExecutionId);
            if (existingGraph == null)
            {
                currentState.runningGraphs.Add(GraphExecutionId, ExecutingGraph);
            }

            return currentState;
        }

        public GraphStreamState<T> GraphStateEvents_OnGraphExecuted_RemoveFromRunningGraphs(FlowAction<ExecutableGraphResult<T>> graphExecutionAction, GraphStreamState<T> currentState)
        {
            var graphExecutedResult = graphExecutionAction.Parameters;
            var GraphExecutionId = graphExecutionAction.Parameters.GraphExecutionId;
            var existingGraph = currentState.runningGraphs.GetValueOrDefault(GraphExecutionId);
            if (existingGraph != null)
            {
                currentState.runningGraphs.Remove(GraphExecutionId);
            }

            return currentState;
        }




    }
}
