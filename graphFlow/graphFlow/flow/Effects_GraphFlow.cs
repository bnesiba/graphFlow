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
        };

        //Effect Methods
        public FlowActionBase OnGraphExecution_ExecuteStartNode_ResolveGraphExecuted(FlowAction<ExecutableGraph<T>> executeGraphAction)
        {
            bool success = false;
            GraphNode<T> nodeToExecute = executeGraphAction.Parameters.startNode;
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

            return Actions.GraphExecuted(executeGraphAction.Parameters, success);
        }

        public FlowActionBase OnNodeExecution_ExecuteNode_ResolveNodeExecuted(FlowAction<GraphNode<T>> executeNodeAction)
        {
            GraphNode<T> nodeToExecute = executeNodeAction.Parameters;
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
            GraphNodeResult<T> result = new GraphNodeResult<T> { nodeExecuted = nodeToExecute, success = success };
            if (success)
            {
                result.nodeOutput = nodeResult;
            }

            return Actions.NodeExecuted(result);
        }

        public FlowActionBase OnNodeExecuted_EvaluateEdges_ResolveNodeSubTreeComplete(FlowAction<GraphNodeResult<T>> nodeExecutedAction)
        {
            var subtreeCompleteAction = Actions.NodeSubtreeComplete(nodeExecutedAction.Parameters);

            //If node failed, don't run edges/futher nodes
            if (!nodeExecutedAction.Parameters.success)
            {
                return subtreeCompleteAction;
            }

            var nodeCompleted = nodeExecutedAction.Parameters.nodeExecuted;
            //evaluate edges
            //TODO: break out into edge execution actions for better tracking.
            var edgesToEvaluate = nodeCompleted.edges;
            try
            {
                T stateData = _flowStateData.CurrentState(StateObjectSelectors<T>.GetStateData);
                foreach (var edge in edgesToEvaluate)
                {
                    if (edge.evaluation(stateData))
                    {
                        _flowActionHandler.ResolveAction(Actions.NodeExecution(edge.targetNode));
                    }
                }
            }
            catch (Exception e)
            {
                //probably do somthing, right?
                Console.WriteLine(e);
            }
            //run nodes that should be run
            return subtreeCompleteAction;
        }

    }
}
