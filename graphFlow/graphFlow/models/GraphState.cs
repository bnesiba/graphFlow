using GraphFlow.checkpointing;
using GraphFlow.flow;
using GraphFlow.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graphFlow.models
{
    //TODO: consider removing stateObject and maybe the whole generic graphstate model. 
    public class GraphState<T> : GraphStateBase
    {
        public GraphState(T state) : base()
        {
            stateObject = state;
        }

        public GraphState() : base()
        {
            stateObject = default(T);
        }

        public T stateObject { get; set; }

    }

    public class GraphState : GraphStateBase
    {
        public GraphState() : base() { }
    }

    public abstract class GraphStateBase
    {
        public Guid id { get; set; }
        public Guid threadId { get; set; }
        public List<GraphStateEvent> graphStateEvents { get; set; }

        public GraphStateBase()
        {
            id = Guid.NewGuid();
            threadId = Guid.NewGuid();
            graphStateEvents = new List<GraphStateEvent>();
        }
    }

    public class GraphRunState<T>
    {
        public Guid Id { get; init; }
        public Guid ThreadId { get; init; }

        public Dictionary<Guid, GraphRun> GraphRuns { get; set; }
        public Dictionary<Guid, NodeRun> NodeRuns { get; set; }
        public Dictionary<Guid, EdgeRun> EdgeRuns { get; set; }
        public Dictionary<Guid, string> CheckPoints { get; set; }
        public List<GraphEvent> GraphEvents { get; set; }
        public Guid CurrentCheckpoint {  get; set; }

        public GraphRunState()
        {
            Id = Guid.NewGuid();
            ThreadId = Guid.NewGuid();
            GraphEvents = new List<GraphEvent>();
            NodeRuns = new Dictionary<Guid, NodeRun>();
            EdgeRuns = new Dictionary<Guid, EdgeRun>();
            GraphRuns = new Dictionary<Guid, GraphRun>();
            CheckPoints = new Dictionary<Guid, string>();
            CurrentCheckpoint = Guid.Empty;
        }

    }

    public class GraphRun//<T>?
    {
        public Guid ExecutionId { get; init; }
        public Guid GraphId { get; init; }
        public Guid Input { get; set; }
        public Guid Output { get; set; }
        public bool? Succeeded { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }


    public class NodeRun//<T>?
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public Guid ExecutionId { get; init; }
        public Guid NodeId { get; init; }
        public string NodeName { get; init; }
        public Guid Input {  get; set; }
        public Guid Output {  get; set; }
        public bool? Succeeded { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class EdgeRun//<T>?
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public Guid ExecutionId { get; init; }
        public string EdgeName { get; set; }
        public Guid SourceNodeId { get; init; }
        public Guid TargetNodeId { get; init; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid Input { get; set; }
        public bool Evaluation { get; set; }//TODO: remove? This will always be true unless we log every edge eval...
    }

    public class GraphEvent
    {
        public Guid GraphItemId { get; init; }
        public string EventType { get; init; }
        public DateTime EventTime { get; set; }

        public GraphEvent(Guid graphItemId, string eventType, DateTime eventTime)
        {
            GraphItemId = graphItemId;
            EventType = eventType;
            EventTime = eventTime;
        }
    }

    //TODO: enum?
    public static class GraphEventTypes
    {
        public static readonly string GraphStarted = "GraphStarted";
        public static readonly string GraphCompleted = "GraphCompleted";
        public static readonly string NodeStarted = "NodeStarted";
        public static readonly string NodeCompleted = "NodeCompleted";
        public static readonly string EdgeEvaluationStarted = "EdgeStarted";
        public static readonly string EdgeEvaluationCompleted = "EdgeEvaluationCompleted";
    }

    public static class GraphRunExtensions
    {
        public static Guid AddCheckpoint<T>(this GraphRunState<T> graphState, T stateObject)
        {
            var serializer = new GraphRunSerializer<T>();
            string serializedObject = serializer.SerializeGraphRun(stateObject);
            Guid checkpointId = Guid.NewGuid();
            graphState.CheckPoints.Add(checkpointId, serializedObject);
            graphState.CurrentCheckpoint = checkpointId;
            return checkpointId;
        }

        public static void AddGraphStarted<T>(this GraphRunState<T> graphState, ExecutableGraph<T> graphRunning, Guid graphExecutionId)
        {

        }

        public static Guid AddGraphComplete<T>(this GraphRunState<T> graphState, Ex)



        public static void AddNodeStarted<T>(this GraphRunState<T> graphState, GraphNode<T> nodeRunning, Guid nodeExecutionId)
        {
            if(graphState.CurrentCheckpoint == Guid.Empty)
            {
                //TODO: throw errors - this shouldn't happen but currently will until finished.
                return;
            }
            var startTime = DateTime.UtcNow;
            NodeRun<T> nodeStarting = new NodeRun<T>()
            {
                NodeId = nodeRunning.id,
                NodeName  = nodeRunning.name,
                Input = graphState.CurrentCheckpoint,
                StartTime = startTime
            };
            graphState.NodeRuns.Add(nodeStarting.ExecutionId, nodeStarting);
            graphState.GraphEvents.Add(new GraphEvent(nodeStarting.Id, GraphEventTypes.NodeStarted, startTime));
        }

        public static Guid AddNodeComplete<T>(this GraphRunState<T> graphState, Guid nodeExecutionId, GraphNodeResult<T> nodeResult)
        {
            Guid checkpointId = graphState.AddCheckpoint(nodeResult.NodeOutput);
            var completeTime = DateTime.UtcNow;
            var NodeRun = graphState.NodeRuns[nodeExecutionId];
            NodeRun.EndTime = completeTime;
            NodeRun.Succeeded = nodeResult.Success;
            NodeRun.Output = checkpointId;
            graphState.CurrentCheckpoint = checkpointId;
            graphState.GraphEvents.Add(new GraphEvent(NodeRun.ExecutionId, GraphEventTypes.NodeCompleted, completeTime));
            return checkpointId;
        }

        public static void AddEdgeStarted<T>(this GraphRunState<T> graphState, GraphEdge<T> edgeExecuting)
        {
            if (graphState.CurrentCheckpoint == Guid.Empty)
            {
                //TODO: throw errors - this shouldn't happen but currently will until finished.
                return;
            }
            var startTime = DateTime.UtcNow;
            EdgeRun edgeRun = new EdgeRun()
            {
                StartTime = startTime,
                Input = graphState.CurrentCheckpoint,

            };
            graphState.EdgeRuns.Add(edgeRun.Id, edgeRun);
            graphState.GraphEvents.Add(new GraphEvent(edgeRun.Id, GraphEventTypes.EdgeEvaluationStarted, startTime));
        }

        public static void AddEdgeCompleted<T>(this GraphRunState<T> graphState, GraphEdgeResult<T> edgeCompleted)
        {
            
        }


    }


}
