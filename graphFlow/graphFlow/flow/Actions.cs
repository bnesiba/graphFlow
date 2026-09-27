using ActionFlow.Models;
using GraphFlow.models;
using GraphFlow.persistence.models;

namespace GraphFlow.flow
{
    //TODO: update action/effects implementation so that inputs can be non-nullable without breaking effects
    public static class Actions
    {
        public static FlowAction<T> UpdateFlowState<T>(T? initialState = default) => new FlowAction<T> { Name = "InitializeFlowState", Parameters = initialState };
        public static FlowAction<ExecutableGraphRequest<T>> GraphExecution<T>(ExecutableGraph<T>? graph = null) => new FlowAction<ExecutableGraphRequest<T>> { Name = "ExecuteGraph", Parameters = new ExecutableGraphRequest<T> { GraphExecutionId = Guid.NewGuid(), ExecutingGraph = graph } };

        public static FlowAction<ExecutableGraphResult<T>> GraphExecuted<T>(ExecutableGraphResult<T>? graph = null, bool success = false) => new FlowAction<ExecutableGraphResult<T>> { Name = "GraphExecuted", Parameters = graph };

        public static FlowAction<GraphNodeRequest<T>> NodeExecution<T>(GraphNode<T>? node = null) => new FlowAction<GraphNodeRequest<T>> { Name = "ExecuteNode", Parameters = new GraphNodeRequest<T> { ExecutionId = Guid.NewGuid(), NodeExecuting = node } };

        public static FlowAction<GraphNodeResult<T>> NodeExecuted<T>(GraphNodeResult<T>? result = null) => new FlowAction<GraphNodeResult<T>> { Name = "NodeExecuted", Parameters = result };

        public static FlowAction<GraphNodeResult<T>> NodeSubtreeComplete<T>(GraphNodeResult<T>? result = null) => new FlowAction<GraphNodeResult<T>> { Name = "NodeSubtreeComplete", Parameters = result };
        
        public static FlowAction<GraphEdgeRequest<T>> EdgeEvaluation<T>(GraphEdge<T>? edge = null) => new FlowAction<GraphEdgeRequest<T>> { Name = "EvaluateEdge", Parameters = new GraphEdgeRequest<T> { ExecutionId = Guid.NewGuid(), EdgeExecuting = edge } };
        
        public static FlowAction<GraphEdgeResult<T>> EdgeEvaluated<T>(GraphEdgeResult<T>? edge = null) => new FlowAction<GraphEdgeResult<T>> { Name = "EdgeEvaluated", Parameters = edge };
        
        public static FlowAction<GraphEdgeResult<T>> EdgeNotFollowed<T>(GraphEdgeResult<T>? edge = null) => new FlowAction<GraphEdgeResult<T>> { Name = "EdgeSubtreeComplete", Parameters = edge };

        public static FlowAction<GraphExecutionData<T>> RunPersisted<T>(GraphExecutionData<T>? record = null) => new FlowAction<GraphExecutionData<T>> { Name = "RunPersisted", Parameters = record };
    }
}
