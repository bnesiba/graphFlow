using ActionFlow;
using ActionFlow.Models;
using GraphFlow.models;

namespace GraphFlow.flow
{
    public class GraphStreamEffects<T> : IFlowStateEffects
    {
        private FlowStateData<GraphStreamState<T>> _graphStreamData;
        private FlowStateData<GraphFlowState<T>> _flowStateData;
        private FlowStateData<GraphOrchestrationState<T>> _graphStateData;

        public GraphStreamEffects(FlowStateData<GraphStreamState<T>> streamData, FlowStateData<GraphFlowState<T>> flowData, FlowStateData<GraphOrchestrationState<T>> graphData)
        {
            _graphStreamData = streamData;
            _flowStateData = flowData;
            _graphStateData = graphData;
        }
        List<IFlowEffectBase> IFlowStateEffects.SideEffects => new List<IFlowEffectBase>
        {
           this.effect(OnGraphExecution_ResolveGraphExecutionProgress, Actions.GraphExecution<T>()),
           this.effect(OnNodeExecution_ResolveGraphExecutionProgress, Actions.NodeExecution<T>()),
           this.effect(OnNodeExecuted_ResolveGraphExecutionProgress, Actions.NodeExecuted<T>()),
           this.effect(OnEdgeEvaluation_ResolveGraphExecutionProgress, Actions.EdgeEvaluation<T>()),
           this.effect(OnEdgeEvaluated_ResolveGraphExecutionProgress, Actions.EdgeEvaluated<T>()),
           this.effect(OnGraphExecuted_ResolveGraphExecutionProgress, Actions.GraphExecuted<T>()),
           this.effect(OnUpdateFlowState_ResolveGraphExecutionProgress, Actions.UpdateFlowState<T>()),
           this.effect(OnGraphExecutionProgress_ReportGraphProgress_ResolveProgressReported, Actions.GraphExecutionProgress<T>())
        };

        private GraphProgressEventArgs<T> GenerateEventArgs(GraphProgressType eventType, string GraphObjectName, Guid graphExecutionId)
        {
            var stateObject = _flowStateData.CurrentState(StateObjectSelectors<T>.SelectByExecution(graphExecutionId));
            var graphState = _graphStateData.CurrentState(GraphStateSelectors<T>.SelectByExecution(graphExecutionId));

            var graphProgress = new GraphProgressEventArgs<T>
            {
                EventType = eventType,
                GraphObjectName = GraphObjectName,
                GraphExecutionId = graphExecutionId,
                GraphRunState = graphState,
                GraphValue = stateObject
            };
            return graphProgress;
        }

        //Effect Methods
        public FlowActionBase OnGraphExecutionProgress_ReportGraphProgress_ResolveProgressReported(FlowAction<GraphProgressEventArgs<T>> graphProgressAction)
        {
            var progressInfo = graphProgressAction.Parameters;
            var reportingGraph = _graphStreamData.CurrentState(GraphStreamSelectors<T>.SelectByExecution(progressInfo.GraphExecutionId));
            reportingGraph?.ReportGraphProgress(progressInfo);
            return Actions.GraphProgressReported(progressInfo.GraphExecutionId, progressInfo.GraphRunState.CurrentCheckpoint);
        }

        public FlowActionBase OnGraphExecution_ResolveGraphExecutionProgress(FlowAction<ExecutableGraphRequest<T>> executeGraphAction)
        {
            Guid graphExecutionId = executeGraphAction.Parameters.GraphExecutionId;
            var graphProgress = GenerateEventArgs(GraphProgressType.GraphStart,"GraphStart", graphExecutionId);
            return Actions.GraphExecutionProgress(graphProgress);
        }

        public FlowActionBase OnNodeExecution_ResolveGraphExecutionProgress(FlowAction<GraphNodeRequest<T>> executeNodeAction)
        {
            var nodeExecution = executeNodeAction.Parameters;
            var graphProgress = GenerateEventArgs(GraphProgressType.NodeStart, nodeExecution.NodeExecuting.name, nodeExecution.GraphExecutionId);
            return Actions.GraphExecutionProgress(graphProgress);
        }

        public FlowActionBase OnNodeExecuted_ResolveGraphExecutionProgress(FlowAction<GraphNodeResult<T>> nodeExecutedAction)
        {
            var nodeComplete = nodeExecutedAction.Parameters;
            var graphProgress = GenerateEventArgs(GraphProgressType.NodeComplete, nodeComplete.NodeExecuted.name, nodeComplete.GraphExecutionId);
            return Actions.GraphExecutionProgress(graphProgress);
        }

        public FlowActionBase OnEdgeEvaluation_ResolveGraphExecutionProgress(FlowAction<GraphEdgeRequest<T>> edgeEvaluationAction)
        {
            var edgeEvaluation = edgeEvaluationAction.Parameters;
            var graphProgress = GenerateEventArgs(GraphProgressType.EdgeStart, edgeEvaluation.EdgeExecuting.name, edgeEvaluation.GraphExecutionId);
            return Actions.GraphExecutionProgress(graphProgress);
        }

        public FlowActionBase OnEdgeEvaluated_ResolveGraphExecutionProgress(FlowAction<GraphEdgeResult<T>> edgeEvaluationAction)
        {
            var edgeComplete = edgeEvaluationAction.Parameters;
            var graphProgress = GenerateEventArgs(GraphProgressType.EdgeComplete, edgeComplete.EdgeExecuted.name, edgeComplete.GraphExecutionId);
            return Actions.GraphExecutionProgress(graphProgress);
        }

        public FlowActionBase OnGraphExecuted_ResolveGraphExecutionProgress(FlowAction<ExecutableGraphResult<T>> edgeEvaluationAction)
        {
            Guid graphExecutionId = edgeEvaluationAction.Parameters.GraphExecutionId;
            var graphProgress = GenerateEventArgs(GraphProgressType.GraphComplete, "GraphEnd", graphExecutionId);
            return Actions.GraphExecutionProgress(graphProgress);
        }

        public FlowActionBase OnUpdateFlowState_ResolveGraphExecutionProgress(FlowAction<StateUpdateRequest<T>> edgeEvaluationAction)
        {
            Guid graphExecutionId = edgeEvaluationAction.Parameters.GraphExecutionId;
            var graphProgress = GenerateEventArgs(GraphProgressType.StateChange, "StateUpdate", graphExecutionId);
            return Actions.GraphExecutionProgress(graphProgress);
        }
    }
}
