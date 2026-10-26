using ActionFlow;
using GraphFlow.models;
using GraphFlow.persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphFlow.util
{
    public class GraphBuilder<T>
    {
        private FlowState _flowState;
        private FlowStateData<GraphFlowState<T>> _flowStateData;
        private FlowStateData<GraphOrchestrationState<T>> _graphStateData;
        private PersistenceManager<T> _persistenceManager;

        public GraphBuilder(FlowState flowState, FlowStateData<GraphFlowState<T>> flowStateData, FlowStateData<GraphOrchestrationState<T>> graphStateData, PersistenceManager<T> persistence)
        {
            _flowState = flowState;
            _flowStateData = flowStateData;
            _graphStateData = graphStateData;
            _persistenceManager = persistence;
        }

        public ExecutableGraph<T> GetExecutableGraph()
        {
            return new ExecutableGraph<T>(_flowState, _flowStateData,_graphStateData, _persistenceManager);
        }

    }
}
