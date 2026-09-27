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
    //TODO: !Important! Update to handle new state object.
    public class GraphPersistenceEffects<T> : IFlowStateEffects
    {
        private FlowStateData<T> _stateObjectData;
        private FlowStateData<GraphRunState<T>> _graphStateData;
        IGraphFlowPersistence<T> flowPersistence;
        public GraphPersistenceEffects(FlowStateData<T> stateObjData, FlowStateData<GraphRunState<T>> graphData, IGraphFlowPersistence<T> persistence) 
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
            var stateObjectSnapshot = _stateObjectData.CurrentState(StateObjectSelectors<T>.GetStateData);
            var graphStateSnapshot = _graphStateData.CurrentState(StateObjectSelectors<GraphRunState<T>>.GetStateData);//TODO: get correctly
            GraphExecutionData<T> record = new GraphExecutionData<T>
            {
                StateObject = stateObjectSnapshot,
                GraphData = null,//TODO: can't be null?
            };
            flowPersistence.StoreGraphSnapshot(record);
            return Actions.RunPersisted(record);
        }
    }
}
