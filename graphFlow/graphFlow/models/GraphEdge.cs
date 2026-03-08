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

    public class GraphEdge : GraphEdgeBase
    {
        public GraphNode targetNode { get; set; }
        public Func<bool> evaluation { get; set; }
    }

    public abstract class GraphEdgeBase
    {
        public Guid id { get; set; }

        public string name { get; set; }
    }
}
