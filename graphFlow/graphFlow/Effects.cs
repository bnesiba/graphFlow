using ActionFlow;
using ActionFlow.Models;
using graphFlow.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graphFlow
{
    public class Effects<T> : IFlowStateEffects
    {
        private FlowStateData<GraphState<T>> _flowStateData;
        private FlowActionHandler _flowActionHandler;

        public Effects(FlowActionHandler flowActionHandler, FlowStateData<GraphState<T>> stateData)
        {
            _flowActionHandler = flowActionHandler;
            _flowStateData = stateData;
        }
        List<IFlowEffectBase> IFlowStateEffects.SideEffects => new List<IFlowEffectBase>
        {
           this.effect(OnGraphExecution_ExecuteStartNode_ResolveGraphExecuted, Actions.GraphExecution<T>()),
           this.effect(OnNodeExecution_ExecuteNode_ResolveNodeExecuted, Actions.NodeExecution<T>())
        };

        //Effect Methods
        public FlowActionBase OnGraphExecution_ExecuteStartNode_ResolveGraphExecuted(FlowAction<ExecutableGraph<T>> executeGraphAction)
        {
            bool success = false;
            GraphNode<T> nodeToExecute = executeGraphAction.Parameters.startNode;
            try
            {
                _flowActionHandler.ResolveAction(Actions.NodeExecution<T>(nodeToExecute));
                success = true;

            }
            catch (Exception ex)
            {
                //TODO: log or include error or both
                success = false;
            }

            return Actions.GraphExecuted<T>(executeGraphAction.Parameters, success);
        }

        public FlowActionBase OnNodeExecution_ExecuteNode_ResolveNodeExecuted(FlowAction<GraphNode<T>> executeNodeAction)
        {
            GraphNode<T> nodeToExecute = executeNodeAction.Parameters;
            bool success = false;
            T? nodeResult = default;
            try
            {
                //get state data
                GraphState<T> stateData = _flowStateData.CurrentState(Selectors<T>.GetStateData);
                nodeResult = nodeToExecute.nodeFunction(stateData.stateObject);
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

        public FlowActionBase OnNodeExecuted_EvaluateEdges_ResolveNodeComplete(FlowAction<GraphNodeResult<T>> nodeExecutedAction)
        {
            return default;
        }

    }
}
