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
           this.effect(OnGraphExecution_ExecuteGraph_ResolveGraphExecuted, Actions.GraphExecution<T>()),
        };

        //Effect Methods
        public FlowActionBase OnGraphExecution_ExecuteGraph_ResolveGraphExecuted(FlowAction<ExecutableGraph<T>> executeGraphAction)
        {
            GraphNode<T> nodeToExecute = executeGraphAction.Parameters.startNode;

            _flowActionHandler.ResolveAction(Actions.NodeExecution<T>(nodeToExecute));

            return Actions.GraphExecuted<T>(executeGraphAction.Parameters);
        }
    }
}
