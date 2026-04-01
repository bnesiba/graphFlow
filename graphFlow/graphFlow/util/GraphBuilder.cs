using ActionFlow;
using graphFlow.models;
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

        public GraphBuilder(FlowState flowState, FlowStateData<T> flowStateData)
        {
            _flowState = flowState;
            _flowStateData = flowStateData;
        }

        public ExecutableGraph<T> GetExecutableGraph()
        {
            return new ExecutableGraph<T>(_flowState, _flowStateData);
        }

    }
}
