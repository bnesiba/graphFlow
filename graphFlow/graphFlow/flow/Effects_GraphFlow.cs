using ActionFlow;
using ActionFlow.Models;
using graphFlow.models;

namespace GraphFlow.flow
{
    public class GraphFlowEffects<T> : IFlowStateEffects
    {
        private FlowStateData<T> _flowStateData;
        private FlowActionHandler _flowActionHandler;

        public GraphFlowEffects(FlowActionHandler flowActionHandler, FlowStateData<T> stateData)
        {
            _flowActionHandler = flowActionHandler;
            _flowStateData = stateData;
        }
        List<IFlowEffectBase> IFlowStateEffects.SideEffects => new List<IFlowEffectBase>
        {
           this.effect(OnGraphExecution_ExecuteStartNode_ResolveGraphExecuted, Actions.GraphExecution<T>()),
           this.effect(OnNodeExecution_ExecuteNode_ResolveNodeExecuted, Actions.NodeExecution<T>()),
           this.effect(OnNodeExecuted_EvaluateEdges_ResolveNodeSubTreeComplete, Actions.NodeExecuted<T>()),
           this.effect(OnEdgeEvaluation_EvaluateEdge_ResolveEdgeEvaluated, Actions.EdgeEvaluation<T>()),
           this.effect(OnEdgeEvaluated_IfShouldContinue_ResolveNodeExecution, Actions.EdgeEvaluated<T>())
        };

        //Effect Methods
        public FlowActionBase OnGraphExecution_ExecuteStartNode_ResolveGraphExecuted(FlowAction<ExecutableGraphRequest<T>> executeGraphAction)
        {
            bool success = false;
            Guid executionId = executeGraphAction.Parameters.GraphExecutionId;
            ExecutableGraph<T> executingGraph = executeGraphAction.Parameters.ExecutingGraph;
            GraphNode<T> nodeToExecute = executingGraph.startNode;
            try
            {
                _flowActionHandler.ResolveAction(Actions.NodeExecution(nodeToExecute));
                success = true;

            }
            catch (Exception ex)
            {
                //TODO: log or include error or both
                success = false;
            }
            T stateData = _flowStateData.CurrentState(StateObjectSelectors<T>.GetStateData);
            ExecutableGraphResult<T> graphResult = new ExecutableGraphResult<T>
            {
                GraphExecutionId = executionId,
                GraphExecuted = executingGraph,
                GraphOutput = stateData,
                Success = success,
                ErrorMessage = ""
            };

            return Actions.GraphExecuted(graphResult);
        }

        public FlowActionBase OnNodeExecution_ExecuteNode_ResolveNodeExecuted(FlowAction<GraphNodeRequest<T>> executeNodeAction)
        {
            GraphNode<T> nodeToExecute = executeNodeAction.Parameters.NodeExecuting;
            var executionId = executeNodeAction.Parameters.ExecutionId;
            bool success = false;
            T? nodeResult = default;
            try
            {
                //get state data
                T stateData = _flowStateData.CurrentState(StateObjectSelectors<T>.GetStateData);
                nodeResult = nodeToExecute.nodeFunction(stateData);
                success = true;
            }
            catch (Exception e)
            {
                //TODO: log and/or include error
                success = false;
            }
            GraphNodeResult<T> result = new GraphNodeResult<T> 
            { 
                ExecutionId = executionId, 
                NodeExecuted = nodeToExecute, 
                Success = success 
            };

            if (success)
            {
                result.NodeOutput = nodeResult;
            }

            return Actions.NodeExecuted(result);
        }

        public FlowActionBase OnNodeExecuted_EvaluateEdges_ResolveNodeSubTreeComplete(FlowAction<GraphNodeResult<T>> nodeExecutedAction)
        {
            var subtreeCompleteAction = Actions.NodeSubtreeComplete(nodeExecutedAction.Parameters);

            //If node failed, don't run edges/futher nodes
            if (!nodeExecutedAction.Parameters.Success)
            {
                return subtreeCompleteAction;
            }

            var nodeCompleted = nodeExecutedAction.Parameters.NodeExecuted;
            //evaluate edges
            var edgesToEvaluate = nodeCompleted.edges;
            try
            {
                foreach (var edge in edgesToEvaluate)
                {
                    _flowActionHandler.ResolveAction(Actions.EdgeEvaluation(edge));
                }
            }
            catch (Exception e)
            {
                //TODO: probably do somthing, right?
                Console.WriteLine(e);
            }
            return subtreeCompleteAction;
        }

        public FlowActionBase OnEdgeEvaluation_EvaluateEdge_ResolveEdgeEvaluated(FlowAction<GraphEdgeRequest<T>> edgeEvaluationAction)
        {
            GraphEdge<T> edge = edgeEvaluationAction.Parameters.EdgeExecuting;
            var edgeExecutionId = edgeEvaluationAction.Parameters.ExecutionId;
            T stateData = _flowStateData.CurrentState(StateObjectSelectors<T>.GetStateData);
            bool evalResult = edge.evaluation(stateData);

            var edgeResult = new GraphEdgeResult<T>
            {
                ExecutionId = edgeExecutionId,
                EdgeExecuted = edge,
                ShouldContinue = evalResult,
                Succeeded = true, //TODO: handle failed edges probably
                ErrorMessage = null
            };

            return Actions.EdgeEvaluated(edgeResult);
        }

        public FlowActionBase OnEdgeEvaluated_IfShouldContinue_ResolveNodeExecution(FlowAction<GraphEdgeResult<T>> edgeEvaluationAction)
        {
            //TODO: handle failure differently
            var edgeResult = edgeEvaluationAction.Parameters;
            if (edgeResult.ShouldContinue)
            {
                return Actions.NodeExecution(edgeResult.EdgeExecuted.targetNode);
            }
            else
            {
                return Actions.EdgeNotFollowed(edgeResult);
            }
        }
    }
}
