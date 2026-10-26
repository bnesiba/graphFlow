using ActionFlow;
using ActionFlow.Models;
using GraphFlow.models;
using GraphFlow.flow;
using GraphFlow.persistence.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphFlow.persistence
{
    public class GraphPersistenceEffects<T> : IFlowStateEffects
    {
        private FlowStateData<GraphFlowState<T>> _stateObjectData;
        private FlowStateData<GraphOrchestrationState<T>> _graphStateData;
        IGraphFlowPersistence<T> flowPersistence;
        public GraphPersistenceEffects(FlowStateData<GraphFlowState<T>> stateObjData, FlowStateData<GraphOrchestrationState<T>> graphData, IGraphFlowPersistence<T> persistence) 
        {
            _stateObjectData = stateObjData;
            _graphStateData = graphData;
            flowPersistence = persistence;
        }
        
        //TODO: multiple persistence rhythms/schedules? 
        public List<IFlowEffectBase> SideEffects => new List<IFlowEffectBase>
        {
            this.effect(OnGraphExecuted_PersistResults_ResolveResultsPersisted, Actions.GraphExecuted<T>())
        };

        //TODO: fix for new models and stuff
        public FlowActionBase OnGraphExecuted_PersistResults_ResolveResultsPersisted(FlowAction<ExecutableGraphResult<T>> graphExecuted)
        {
            Guid graphExecutionId = graphExecuted.Parameters.GraphExecutionId;
            var stateObjectSnapshot = _stateObjectData.CurrentState(StateObjectSelectors<T>.SelectByExecution(graphExecutionId));
            var graphStateSnapshot = _graphStateData.CurrentState(GraphStateSelectors<T>.SelectByExecution(graphExecutionId));
            GraphExecutionData<T> record = new GraphExecutionData<T>
            {
                StateObject = stateObjectSnapshot,
                GraphData = null,//TODO: null?
            };
            flowPersistence.StoreGraphSnapshot(record);
            return Actions.RunPersisted(record);
        }
    }
}
