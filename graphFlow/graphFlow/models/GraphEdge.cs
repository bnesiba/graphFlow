using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graphFlow.models
{
    public class GraphEdge<T> : GraphEdgeBase
    {
        public GraphNode<T> targetNode { get; set; }

        public Func<T, bool> evaluation {  get; set; }

    }


    //TODO: simplify, untyped graphs no longer supported.
    public class GraphEdge : GraphEdgeBase
    {
        public GraphNode targetNode { get; set; }
        public Func<bool> evaluation { get; set; }
    }

    //TODO: simplify, untyped graphs no longer supported.
    public abstract class GraphEdgeBase
    {
        public Guid id { get; set; }

        public string name { get; set; }
    }

    public class GraphEdgeResult<T>
    {
        public Guid ExecutionId { get; set; }
        public GraphEdge<T> EdgeExecuted { get; set; }
        public bool ShouldContinue { get; set; }
        public bool? Succeeded { get; set; }
        public string? ErrorMessage {  get; set; }
        
    }

    public class GraphEdgeRequest<T>
    {
        public Guid ExecutionId { get; set; }
        public GraphEdge<T> EdgeExecuting { get; set; }
    }
}
