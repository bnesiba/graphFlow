using ActionFlow;

namespace graphFlow.models
{
    public class ExecutableGraph<T> : ExecutableGraphBase
    {
        private FlowState _flowState;
        private FlowStateData<GraphState<T>> _flowStateData;
        public Dictionary<string, GraphNode<T>> graphNodes { get; set; }
        public List<GraphEdge<T>> graphEdges { get; set; }
        public GraphNode<T> startNode { get; set; }

        public ExecutableGraph(FlowState flowState, FlowStateData<GraphState<T>> flowStateData)
        {
            _flowState = flowState;
            _flowStateData = flowStateData;
        }

        public T ExecuteGraph(T graphInput)
        {
            //call action to run graph.
            //get graph state
            //return state

            _flowState.ResolveAction(Actions.GraphExecution(this));
            T currentState = _flowStateData.CurrentState(Selectors<T>.GetStateData);
            return currentState;
        }

    }

    public class ExecutableGraph : ExecutableGraphBase
    {
        private FlowState _flowState;
        public Dictionary<string, GraphNode> graphNodes { get; set; }
        public List<GraphEdge> graphEdges { get; set; }
        public GraphNode startNode { get; set; }

        public void ExecuteGraph()
        {
            _flowState.ResolveAction(Actions.GraphExecution(this));
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

    //public class ExecutableGraphResult
    //{
    //    public ExecutableGraphBase graph {  get; set; }
    //    public bool success { get; set; }
    //}


    //public interface IGraphStateObject<T>
    //{
    //    T startingValue { get; set; }
    //    public T applyChanges()
    //}

    public abstract class GraphStateObject<T>
    {
        public required T InitialValue { get; set; }


        public virtual T Reduce(T oldValue, T newValue)
        {
            return newValue;
        }
        //fields?
    }
}
