using ActionFlow;
using ActionFlow.Models;
using GraphFlow.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphFlow.flow
{
    //TODO: implement me for streaming.
    public class GraphStreamEffects<T> : IFlowStateEffects
    {
        private FlowStateData<T> _flowStateData;
        private FlowStateData<GraphRunState<T>> _graphFlowData;
        private FlowStateData<GraphStreamState<T>> _streamingGraphData;

        public GraphStreamEffects(FlowStateData<T> stateData, FlowStateData<GraphRunState<T>> graphData, FlowStateData<GraphStreamState<T>> streamData) 
        {
            _flowStateData = stateData;
            _graphFlowData = graphData;
            _streamingGraphData = streamData;
        }

        public List<IFlowEffectBase> SideEffects =>  new List<IFlowEffectBase>
        {
           this.effect(OnGraphExecution_ExecuteStartNode_ResolveGraphExecuted, Actions.GraphExecution<T>()),
           this.effect(OnNodeExecution_ResolveGraphProgressUpdate, Actions.NodeExecution<T>()),
           this.effect(OnNodeExecuted_IfEdges_ResolveEdgeEvaluation, Actions.NodeExecuted<T>()),
           this.effect(OnEdgeEvaluation_EvaluateEdge_ResolveEdgeEvaluated, Actions.EdgeEvaluation<T>()),
           this.effect(OnEdgeEvaluated_IfShouldContinue_ResolveNodeExecution, Actions.EdgeEvaluated<T>()),
           this.effect(OnEdgeNotFollowed_IfEdgesRemain_ResolveEdgeEvaluation, Actions.EdgeNotFollowed<T>()),
           this.effect(OnGraphProgressUpdate_TriggerGraphProgressEvent_ResolveProgressUpdated, Actions.GraphProgressUpdate<T>())
        };

        //effects methods
        public FlowActionBase OnNodeExecution_ResolveGraphProgressUpdate(FlowAction<GraphNodeRequest<T>> executeNodeAction)
        {

            return Actions.GraphProgressUpdate(new GraphProgressEventArgs<T>());
        }


        public FlowActionBase OnGraphProgressUpdate_TriggerGraphProgressEvent_ResolveProgressUpdated(FlowAction<GraphProgressEventArgs<T>> graphProgressUpdateAction)
        {
            GraphProgressEventArgs<T> eventData = graphProgressUpdateAction.Parameters;

            return Actions.GraphProgressUpdated(eventData);
        }
    }
}
