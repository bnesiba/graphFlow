using ActionFlow;
using graphFlow.models;
using GraphFlow.persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graphFlow.util
{
    public class GraphBuilder<T>
    {
        private FlowState _flowState;
        private FlowStateData<T> _flowStateData;
        private PersistenceManager<T> _persistenceManager;

        public GraphBuilder(FlowState flowState, FlowStateData<T> flowStateData, PersistenceManager<T> persistence)
        {
            _flowState = flowState;
            _flowStateData = flowStateData;
            _persistenceManager = persistence;
        }

        public ExecutableGraph<T> GetExecutableGraph()
        {
            return new ExecutableGraph<T>(_flowState, _flowStateData, _persistenceManager);
        }

    }
}
