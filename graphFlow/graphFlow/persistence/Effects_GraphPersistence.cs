using ActionFlow;
using ActionFlow.Models;
using graphFlow.models;
using GraphFlow.flow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphFlow.persistence
{
    public class GraphPersistenceEffects<T> : IFlowStateEffects
    {
        private FlowStateData<T> _stateObjectData;
        private FlowStateData<GraphState<T>> _graphStateData;
        IGraphFlowPersistence<T> flowPersistence;
        public GraphPersistenceEffects(FlowStateData<T> stateObjData, FlowStateData<GraphState<T>> graphData, IGraphFlowPersistence<T> persistence) 
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

        public FlowActionBase OnGraphExecuted_PersistResults_ResolveResultsPersisted(FlowAction<ExecutableGraph<T>> graphExecuted)
        {
            var stateObjectSnapshot = _stateObjectData.CurrentState(StateObjectSelectors<T>.GetStateData);
            var graphStateSnapshot = _graphStateData.CurrentState(StateObjectSelectors<GraphState<T>>.GetStateData);
            PersistenceRecord<T> record = new PersistenceRecord<T>
            {
                stateObject = stateObjectSnapshot,
                graphState = graphStateSnapshot,
            };
            flowPersistence.StoreGraphSnapshot(record);
            return Actions.RunPersisted(record);
        }
    }
}
