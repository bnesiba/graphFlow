using GraphFlow.checkpointing;
using GraphFlow.flow;
using GraphFlow.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphFlow.models
{

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
        public Guid Id { get; init; } = Guid.NewGuid();
        public Guid ExecutionId { get; init; }
        public Guid GraphId { get; init; }
        public Guid Input { get; set; }
        public Guid Output { get; set; }
        public bool? Succeeded { get; set; }
        public string? ErrorMessage { get; set; }
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
        public string? ErrorMessage { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class EdgeRun//<T>?
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public Guid ExecutionId { get; init; }
        public string EdgeName { get; set; }
        public string TargetEdgeName { get; set; }
        public Guid SourceNodeId { get; init; }
        public Guid TargetNodeId { get; init; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid Input { get; set; }
        public bool Evaluation { get; set; }
        public bool? Succeeded { get; set; }
        public string? ErrorMessage { get; set; }
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
        public static readonly string GraphStateUpdate = "GraphStateUpdate";
    }

    //TODO: move to new file
    public static class GraphRunExtensions
    {
        public static Guid AddCheckpoint<T>(this GraphRunState<T> graphState, T stateObject)
        {
            var serializer = new GraphRunSerializer<T>();
            string serializedObject = serializer.SerializeGraphRun(stateObject);
            Guid checkpointId = Guid.NewGuid();
            var updateTime = DateTime.UtcNow;
            graphState.CheckPoints.Add(checkpointId, serializedObject);
            graphState.CurrentCheckpoint = checkpointId;
            graphState.GraphEvents.Add(new GraphEvent(checkpointId, GraphEventTypes.GraphStateUpdate, updateTime));
            return checkpointId;
        }

        public static void AddGraphStarted<T>(this GraphRunState<T> graphState, ExecutableGraphRequest<T> graphRequest)
        {
            if(graphState.CurrentCheckpoint == Guid.Empty)
            {
                //TODO: throw errors?
                return;
            }
            var startTime = DateTime.UtcNow;
            var graphId = graphRequest.ExecutingGraph.id ?? Guid.Empty;//TODO: address? shouldn't be null
            var graphExeId = graphRequest.GraphExecutionId;
            GraphRun graphStarting = new GraphRun()
            {
                GraphId = graphId,
                ExecutionId = graphExeId,
                StartTime = startTime,
                Input = graphState.CurrentCheckpoint
            };
            graphState.GraphRuns.Add(graphExeId, graphStarting);
            graphState.GraphEvents.Add(new GraphEvent(graphStarting.Id, GraphEventTypes.GraphStarted, startTime));
        }

        public static void AddGraphComplete<T>(this GraphRunState<T> graphState, ExecutableGraphResult<T> graphResult)
        {
            //TODO: implement
            var graphRun = graphState.GraphRuns[graphResult.GraphExecutionId];
            var completeTime = DateTime.UtcNow;
            graphRun.Output = graphState.CurrentCheckpoint;
            graphRun.EndTime = completeTime;

            graphRun.Succeeded = graphResult.Success;
            graphRun.ErrorMessage = graphResult.ErrorMessage;
        }



        public static void AddNodeStarted<T>(this GraphRunState<T> graphState, GraphNodeRequest<T> graphNodeRequest)
        {
            if(graphState.CurrentCheckpoint == Guid.Empty)
            {
                //TODO: throw errors - this shouldn't happen but currently will until finished.
                return;
            }
            var nodeRunning = graphNodeRequest.NodeExecuting;
            var startTime = DateTime.UtcNow;
            NodeRun nodeStarting = new NodeRun()
            {
                ExecutionId = graphNodeRequest.ExecutionId,
                NodeId = nodeRunning.id,
                NodeName  = nodeRunning.name,
                Input = graphState.CurrentCheckpoint,
                StartTime = startTime
            };
            graphState.NodeRuns.Add(nodeStarting.ExecutionId, nodeStarting);
            graphState.GraphEvents.Add(new GraphEvent(nodeStarting.Id, GraphEventTypes.NodeStarted, startTime));
        }

        //TODO: handle node failure differently?
        public static Guid AddNodeComplete<T>(this GraphRunState<T> graphState, GraphNodeResult<T> nodeResult)
        {
            Guid checkpointId = graphState.AddCheckpoint(nodeResult.NodeOutput);//TODO: only update checkpoint if succeeded?
            var completeTime = DateTime.UtcNow;
            var NodeRun = graphState.NodeRuns[nodeResult.ExecutionId];
            NodeRun.EndTime = completeTime;
            NodeRun.Succeeded = nodeResult.Success;
            NodeRun.ErrorMessage = nodeResult.ErrorMessage;
            NodeRun.Output = checkpointId;
            graphState.CurrentCheckpoint = checkpointId;
            graphState.GraphEvents.Add(new GraphEvent(NodeRun.ExecutionId, GraphEventTypes.NodeCompleted, completeTime));
            return checkpointId;
        }

        public static void AddEdgeStarted<T>(this GraphRunState<T> graphState, GraphEdgeRequest<T> edgeEvalRequest)
        {
            if (graphState.CurrentCheckpoint == Guid.Empty)
            {
                //TODO: throw errors - this shouldn't happen but currently will until finished.
                return;
            }
            var startTime = DateTime.UtcNow;
            var edgeEvaluating = edgeEvalRequest.EdgeExecuting;
            EdgeRun edgeRun = new EdgeRun()
            {
                ExecutionId = edgeEvalRequest.ExecutionId,
                StartTime = startTime,
                Input = graphState.CurrentCheckpoint,
                EdgeName = edgeEvaluating.name,
                TargetEdgeName = edgeEvaluating.targetNode.name,
                SourceNodeId = edgeEvaluating.id,
                TargetNodeId = edgeEvaluating.targetNode.id
            };
            graphState.EdgeRuns.Add(edgeRun.Id, edgeRun);
            graphState.GraphEvents.Add(new GraphEvent(edgeRun.Id, GraphEventTypes.EdgeEvaluationStarted, startTime));
        }

        public static void AddEdgeCompleted<T>(this GraphRunState<T> graphState, GraphEdgeResult<T> edgeCompleted)
        {
            var completeTime = DateTime.UtcNow;
            var executionId = edgeCompleted.ExecutionId;
            var edgeRun = graphState.EdgeRuns[executionId];
            edgeRun.Evaluation = edgeCompleted.ShouldContinue;
            edgeRun.EndTime = completeTime;
            edgeRun.Succeeded = edgeCompleted.Succeeded;
            edgeRun.ErrorMessage = edgeCompleted.ErrorMessage;
            graphState.GraphEvents.Add(new GraphEvent(edgeRun.Id, GraphEventTypes.EdgeEvaluationCompleted, completeTime));
        }


    }


}
