
namespace GraphFlow.models
{
    public class GraphNode<T> : GraphNodeBase
    {
        public Func<T, T> nodeFunction { get; set; }
        public List<GraphEdge<T>> edges { get; set; }
    }

    //TODO: simplify. untyped graph probably unnecessary. 
    public abstract class GraphNodeBase
    {
        public Guid id { get; set; }
        public string name { get; set; }
    }

    public class GraphNodeResult<T>
    {
        public Guid ExecutionId { get; set; }
        public GraphNode<T> NodeExecuted { get; set; }
        public T NodeOutput { get; set; }
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class GraphNodeRequest<T>
    {
        public GraphNode<T> NodeExecuting { get; set; }
        public Guid ExecutionId { get; set; }
    }    
}
