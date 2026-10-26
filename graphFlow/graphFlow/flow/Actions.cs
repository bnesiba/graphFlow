using ActionFlow.Models;
using GraphFlow.models;
using GraphFlow.persistence.models;

namespace GraphFlow.flow
{
    //TODO: update action/effects implementation so that inputs can be non-nullable without breaking effects
    public static class Actions
    {
        public static FlowAction<StateUpdateRequest<T>> UpdateFlowState<T>(StateUpdateRequest<T>? stateUpdate = null) => new FlowAction<StateUpdateRequest<T>> { Name = "UpdateFlowState", Parameters = stateUpdate };
        public static FlowAction<ExecutableGraphRequest<T>> GraphExecution<T>(ExecutableGraph<T>? graph = null, Guid? graphExecutionId = null) => new FlowAction<ExecutableGraphRequest<T>> { Name = "ExecuteGraph", Parameters = new ExecutableGraphRequest<T> { GraphExecutionId = graphExecutionId ?? Guid.NewGuid(), ExecutingGraph = graph } };

        public static FlowAction<ExecutableGraphResult<T>> GraphExecuted<T>(ExecutableGraphResult<T>? graph = null, bool success = false) => new FlowAction<ExecutableGraphResult<T>> { Name = "GraphExecuted", Parameters = graph };

        public static FlowAction<GraphNodeRequest<T>> NodeExecution<T>(GraphNode<T>? node = null, Guid? graphExecutionId = null) => new FlowAction<GraphNodeRequest<T>> { Name = "ExecuteNode", Parameters = new GraphNodeRequest<T> {GraphExecutionId = graphExecutionId ?? Guid.Empty, ExecutionId = Guid.NewGuid(), NodeExecuting = node } };

        public static FlowAction<GraphNodeResult<T>> NodeExecuted<T>(GraphNodeResult<T>? result = null) => new FlowAction<GraphNodeResult<T>> { Name = "NodeExecuted", Parameters = result };

        
        public static FlowAction<GraphEdgeRequest<T>> EdgeEvaluation<T>(GraphEdge<T>? edge = null, GraphNode<T>? sourceNode = null, int index = 0, Guid? graphExecutionId = null) => new FlowAction<GraphEdgeRequest<T>> { Name = "EvaluateEdge", Parameters = new GraphEdgeRequest<T> { GraphExecutionId = graphExecutionId ?? Guid.Empty, ExecutionId = Guid.NewGuid(), SourceNode = sourceNode,  EdgeExecuting = edge, EdgeIndex = index } };
        
        public static FlowAction<GraphEdgeResult<T>> EdgeEvaluated<T>(GraphEdgeResult<T>? edge = null) => new FlowAction<GraphEdgeResult<T>> { Name = "EdgeEvaluated", Parameters = edge };
        
        public static FlowAction<GraphEdgeResult<T>> EdgeNotFollowed<T>(GraphEdgeResult<T>? edge = null) => new FlowAction<GraphEdgeResult<T>> { Name = "EdgeSubtreeComplete", Parameters = edge };
        public static FlowAction NoEdgesFollowed() => new FlowAction { Name = "NoEdgesFolowed" };//TODO: add graphExecutionGuid to make this action usable/tracable/etc. 
        public static FlowAction<GraphExecutionData<T>> RunPersisted<T>(GraphExecutionData<T>? record = null) => new FlowAction<GraphExecutionData<T>> { Name = "RunPersisted", Parameters = record };
    }
}
