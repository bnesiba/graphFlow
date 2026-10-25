using ActionFlow;
using GraphFlow.flow;
using GraphFlow.persistence;
using GraphFlow.persistence.models;

namespace GraphFlow.models
{
    public class ExecutableGraph<T> : ExecutableGraphBase
    {
        private FlowState _flowState;
        private FlowStateData<GraphFlowState<T>> _flowStateData;
        private FlowStateData<GraphOrchestrationState<T>> _graphStateData;
        private PersistenceManager<T> _persistenceManager;
        public Dictionary<string, GraphNode<T>> graphNodes { get; set; }
        public List<GraphEdge<T>> graphEdges { get; set; }
        public GraphNode<T>? startNode { get; set; }

        public ExecutableGraph(FlowState flowState, FlowStateData<GraphFlowState<T>> flowStateData,FlowStateData<GraphOrchestrationState<T>> graphStateData, PersistenceManager<T> persistence):base()
        {
            _flowState = flowState;
            _flowStateData = flowStateData;
            _graphStateData = graphStateData;
            _persistenceManager = persistence;
            graphNodes = new Dictionary<string, GraphNode<T>>();
            graphEdges = new List<GraphEdge<T>>();
            startNode = null;
        }

        public GraphExecutionData<T> ExecuteGraph(T graphInput)
        {
            Guid execution = Guid.NewGuid();
            var updateObj = new StateUpdateRequest<T>{ GraphExecutionId = execution, NewState = graphInput };
            _flowState.ResolveAction(Actions.UpdateFlowState(updateObj));
            return ExecuteGraph(execution);
        }

        public GraphExecutionData<T> ExecuteGraph(Guid? executionGuid = null)
        {
            GraphNode<T>? initialNode = this.startNode;
            if (initialNode == null)
            {
                throw new ArgumentException("Start node must exist to execute graph");
            }
            if(executionGuid == null)
            {
                executionGuid = Guid.NewGuid();
            }
            _flowState.ResolveAction(Actions.GraphExecution(this));
            T currentState = _flowStateData.CurrentState(StateObjectSelectors<T>.SelectByExecution((Guid)executionGuid));
            GraphRunState<T> graphState = _graphStateData.CurrentState(GraphStateSelectors<T>.SelectByExecution((Guid)executionGuid));
            GraphExecutionData<T> graphRun = new GraphExecutionData<T>()
            {
                GraphData = graphState,
                StateObject = currentState
            };
            return graphRun;
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
            GraphNode<T>? initialNode = graphNodes.GetValueOrDefault(name);
            if (initialNode == null)
            {
                throw new ArgumentException("Start node must exist");
            }
            startNode = initialNode;
        }

        public void AddEdge(string startNode, string endNode, Func<T, bool> edgeFunction)
        {
            GraphNode<T>? originNode = graphNodes.GetValueOrDefault(startNode);
            GraphNode<T>? destinationNode = graphNodes.GetValueOrDefault(endNode);
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
            else
            {
                //TODO: errors or something
            }
        }

        public void AddEdge(string startNode, string endNode)
        {
            GraphNode<T>? originNode = graphNodes.GetValueOrDefault(startNode);
            GraphNode<T>? destinationNode = graphNodes.GetValueOrDefault(endNode);
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
            else
            {
                //TODO: errors or something
            }
        }

        private bool _AlwaysRunEdge<T>(T stateObject)
        {
            return true;
        }
    }
    public class ExecutableGraphRequest<T> : GraphOrchestrationObject
    {
        public ExecutableGraph<T> ExecutingGraph { get; set; }
    }

    public class ExecutableGraphResult<T> : GraphOrchestrationObject
    {
        public ExecutableGraph<T> GraphExecuted { get; set; }
        public T GraphOutput { get; set; }
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }

    //TODO: cleanup - stateless graphs no longer supported.
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
}
