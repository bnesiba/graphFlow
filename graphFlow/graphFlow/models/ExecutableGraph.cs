using ActionFlow;
using System.Xml.Linq;

namespace graphFlow.models
{
    public class ExecutableGraph<T> : ExecutableGraphBase
    {
        private FlowState _flowState;
        private FlowStateData<T> _flowStateData;
        public Dictionary<string, GraphNode<T>> graphNodes { get; set; }
        public List<GraphEdge<T>> graphEdges { get; set; }
        public GraphNode<T>? startNode { get; set; }

        public ExecutableGraph(FlowState flowState, FlowStateData<T> flowStateData):base()
        {
            _flowState = flowState;
            _flowStateData = flowStateData;
            graphNodes = new Dictionary<string, GraphNode<T>>();
            graphEdges = new List<GraphEdge<T>>();
            startNode = null;
        }

        //public T ExecuteGraph(T graphInput)
        //{
        //    //call action to run graph.
        //    //get graph state
        //    //return state
        //    GraphNode<T> initialNode = this.startNode;
        //    if (initialNode == null)
        //    {
        //        throw new ArgumentException("Start node must exist to execute graph");
        //    }
        //    //_flowState.ResolveAction(Actions.InitializeFlowState(graphInput));
        //    _flowState.ResolveAction(Actions.GraphExecution(this));
        //    T currentState = _flowStateData.CurrentState(StateObjectSelectors<T>.GetStateData);
        //    return currentState;
        //}

        public T ExecuteGraph()
        {
            GraphNode<T> initialNode = this.startNode;
            if (initialNode == null)
            {
                throw new ArgumentException("Start node must exist to execute graph");
            }
            //_flowState.ResolveAction(Actions.InitializeFlowState(graphInput));
            _flowState.ResolveAction(Actions.GraphExecution(this));
            T currentState = _flowStateData.CurrentState(StateObjectSelectors<T>.GetStateData);
            return currentState;
        }

        public void AddNode(string name, Func<T, T> nodeFunction)
        {
            GraphNode<T> graphNode = new GraphNode<T>
            {
                name = name,
                nodeFunction = nodeFunction,
                edges = new List<GraphEdge<T>>(),
                id = Guid.NewGuid()
            };
            graphNodes.Add(name, graphNode);
        }

        public void SetStartNode(string name)
        {
            GraphNode<T> initialNode = graphNodes.GetValueOrDefault(name);
            if (initialNode == null)
            {
                throw new ArgumentException("Start node must exist");
            }
            startNode = initialNode;
        }

        public void AddEdge(string startNode, string endNode, Func<T, bool> edgeFunction)
        {
            GraphNode<T> originNode = graphNodes.GetValueOrDefault(startNode);
            GraphNode<T> destinationNode = graphNodes.GetValueOrDefault(endNode);
            if(originNode != null && destinationNode != null)
            {
                int edgeCount = originNode.edges.Count;
                GraphEdge<T> graphEdge = new GraphEdge<T>
                {
                    name = $"{startNode}_{endNode}_edge_{edgeCount}",
                    targetNode = destinationNode,
                    evaluation = edgeFunction,
                    id = Guid.NewGuid()
                };
                originNode.edges.Add(graphEdge);
                graphEdges.Add(graphEdge);
            }
        }

        public void AddEdge(string startNode, string endNode)
        {
            GraphNode<T> originNode = graphNodes.GetValueOrDefault(startNode);
            GraphNode<T> destinationNode = graphNodes.GetValueOrDefault(endNode);
            if (originNode != null && destinationNode != null)
            {
                int edgeCount = originNode.edges.Count;
                GraphEdge<T> graphEdge = new GraphEdge<T>
                {
                    name = $"{startNode}_{endNode}_edge_{edgeCount}",
                    targetNode = destinationNode,
                    evaluation = _AlwaysRunEdge,
                    id = Guid.NewGuid()
                };
                originNode.edges.Add(graphEdge);
                graphEdges.Add(graphEdge);
            }
        }

        private bool _AlwaysRunEdge<T>(T stateObject)
        {
            return true;
        }
    }

    //TODO: verify this even works ↓↓ 
    //TODO: implement edge/node methods at base level or something? or just in both places?
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
        public Guid? id { get; set; }

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
