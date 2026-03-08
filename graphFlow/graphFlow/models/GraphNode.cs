using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graphFlow.models
{
    public class GraphNode<T> : GraphNodeBase
    {
        public Func<T, T> nodeFunction { get; set; }
        public List<GraphEdge<T>> edges { get; set; }
    }

    public class GraphNode : GraphNodeBase
    {
        public Action nodeFunction { get; set; }
        public List<GraphEdge> edges { get; set; }
    }

    public abstract class GraphNodeBase
    {
        public Guid id { get; set; }
        public string name { get; set; }
    }

    public class GraphNodeResult<T>: GraphNodeResultBase
    {
        public GraphNode<T> nodeExecuted { get; set; }
        public T nodeOutput { get; set; }
    }

    public class GraphNodeResult: GraphNodeResultBase
    {
        public GraphNode nodeExecuted { get; set; }
    }

    public abstract class GraphNodeResultBase
    {

    }
}
