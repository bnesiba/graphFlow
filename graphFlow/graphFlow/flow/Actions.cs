using ActionFlow.Models;
using graphFlow.models;
using GraphFlow.persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphFlow.flow
{
    //TODO: update action/effects implementation so that inputs can be non-nullable without breaking effects
    public static class Actions
    {
        public static FlowAction<T> UpdateFlowState<T>(T? initialState = default) => new FlowAction<T> { Name = "InitializeFlowState", Parameters = initialState };
        public static FlowAction<ExecutableGraph> GraphExecution(ExecutableGraph? graph = null) => new FlowAction<ExecutableGraph> { Name = "ExecuteGraphStateless", Parameters = graph };
        public static FlowAction<ExecutableGraph<T>> GraphExecution<T>(ExecutableGraph<T>? graph = null) => new FlowAction<ExecutableGraph<T>> { Name = "ExecuteGraph", Parameters = graph};

        public static FlowAction<ExecutableGraph> GraphExecuted(ExecutableGraph? graph = null, bool success = false) => new FlowAction<ExecutableGraph> { Name = "GraphExecutedStateless", Parameters = graph };
        public static FlowAction<ExecutableGraph<T>> GraphExecuted<T>(ExecutableGraph<T>? graph = null, bool success = false) => new FlowAction<ExecutableGraph<T>> { Name = "GraphExecuted", Parameters = graph };

        public static FlowAction<GraphNode> NodeExecution(GraphNode? node = null) => new FlowAction<GraphNode> { Name = "ExcuteNodeStateless", Parameters = node };
        public static FlowAction<GraphNode<T>> NodeExecution<T>(GraphNode<T>? node = null) => new FlowAction<GraphNode<T>> { Name = "ExecuteNode", Parameters = node };

        public static FlowAction<GraphNodeResult> NodeExecuted(GraphNodeResult? result = null) => new FlowAction<GraphNodeResult> { Name = "NodeExecutedStateless", Parameters = result };
        public static FlowAction<GraphNodeResult<T>> NodeExecuted<T>(GraphNodeResult<T>? result = null) => new FlowAction<GraphNodeResult<T>> { Name = "NodeExecuted", Parameters = result };

        public static FlowAction<GraphNodeResult> NodeSubtreeComplete(GraphNodeResult? result = null) => new FlowAction<GraphNodeResult> { Name = "NodeSubtreeCompleteStateless", Parameters = result };
        public static FlowAction<GraphNodeResult<T>> NodeSubtreeComplete<T>(GraphNodeResult<T>? result = null) => new FlowAction<GraphNodeResult<T>> { Name = "NodeSubtreeComplete", Parameters = result };
        
        //TODO: hook these up? ↓ ↓ 
        public static FlowAction<GraphEdge> EdgeEvaluation(GraphEdge? edge = null) => new FlowAction<GraphEdge> { Name = "EvaluateEdgeStateless", Parameters = edge };
        public static FlowAction<GraphEdge<T>> EdgeEvaluation<T>(GraphEdge<T>? edge = null) => new FlowAction<GraphEdge<T>> { Name = "EvaluateEdge", Parameters = edge };


        public static FlowAction<GraphEdgeResult> EdgeEvaluated(GraphEdgeResult? edge = null) => new FlowAction<GraphEdgeResult> { Name = "EdgeEvaluatedStateless", Parameters = edge };
        public static FlowAction<GraphEdgeResult<T>> EdgeEvaluated<T>(GraphEdgeResult<T>? edge = null) => new FlowAction<GraphEdgeResult<T>> { Name = "EdgeEvaluated", Parameters = edge };
        public static FlowAction<GraphEdgeResult<T>> EdgeNotFollowed<T>(GraphEdgeResult<T>? edge = null) => new FlowAction<GraphEdgeResult<T>> { Name = "EdgeSubtreeComplete", Parameters = edge };

        public static FlowAction<PersistenceRecord<T>> RunPersisted<T>(PersistenceRecord<T>? record = null) => new FlowAction<PersistenceRecord<T>> { Name = "RunPersisted", Parameters = record };
    }
}
