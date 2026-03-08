using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graphFlow.models
{
    public class ExecutableGraph<T> : ExecutableGraphBase
    {
        public Dictionary<string, GraphNode<T>> graphNodes { get; set; }
        public List<GraphEdge<T>> graphEdges { get; set; }
        public GraphNode<T> startNode { get; set; }

        public T ExecuteGraph(T graphInput)
        {
            //call action to run graph.
            //get graph state
            //return state
            return default(T);
        }

    }

    public class ExecutableGraph : ExecutableGraphBase
    {
        public Dictionary<string, GraphNode> graphNodes { get; set; }
        public List<GraphEdge> graphEdges { get; set; }
        public GraphNode startNode { get; set; }

        public void ExecuteGraph()
        {
            //call action to run graph
        }

    }

    public abstract class ExecutableGraphBase
    {
        public required Guid id { get; init; }

        public ExecutableGraphBase()
        {
            id = Guid.NewGuid();
        }

        public ExecutableGraphBase(Guid id)
        {
            this.id = id;
        }
    }

}
