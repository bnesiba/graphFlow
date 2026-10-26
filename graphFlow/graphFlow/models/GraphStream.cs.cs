using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphFlow.models
{
    public class GraphStreamState<T> : GraphFlowState<ExecutableGraph<T>>
    {
        //this is to simplify the code because ConcurrentDictionary<Guid, ExecutableGraph<T>> is unwieldy.
    }

    public enum GraphEventType
    {
        GraphStart,
        GraphComplete,
        NodeStart,
        NodeComplete,
        EdgeStart,
        EdgeComplete,
        StateChange
    }

    //TODO: refine - throwing everything in for now
    public class GraphProgressEventArgs<T> : EventArgs
    {
        public Guid GraphExecutionId { get; set; }
        public T GraphValue { get; set; }
        public GraphEventType EventType { get; set; }
        public string GraphObjectName { get; set; }
        public GraphRunState<T> GraphRunState { get; set; }
    }

    public class GraphReportResult
    {
        public Guid GraphExecutionId { get; set; }
        public Guid CurrentCheckpoint { get; set; }
    }
}
