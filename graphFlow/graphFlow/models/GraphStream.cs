using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphFlow.models
{
    public class GraphStreamState<T>
    {
        public ConcurrentDictionary<Guid, ExecutableGraph<T>> runningGraphs;

        public GraphStreamState()
        {
            runningGraphs = new ConcurrentDictionary<Guid, ExecutableGraph<T>>();
        }
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

    public class GraphProgressEventArgs<T>: EventArgs
    {
        public T GraphValue { get; set; }
        public GraphEventType EventType { get; set; }
        public string GraphObjectName { get; set; }
        public GraphRunState<T> GraphRunState { get; set; }
    }
}
