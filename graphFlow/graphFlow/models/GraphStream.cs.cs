namespace GraphFlow.models
{
    public class GraphStreamState<T> : GraphFlowState<ExecutableGraph<T>>
    {
        //this is to simplify the code because ConcurrentDictionary<Guid, ExecutableGraph<T>> is unwieldy and harder to read.
    }

    public enum GraphProgressType
    {
        GraphStart,
        GraphComplete,
        NodeStart,
        NodeComplete,
        EdgeStart,
        EdgeComplete,
        StateChange
    }

    //TODO: refine - tossing everything in for now
    public class GraphProgressEventArgs<T> : EventArgs
    {
        public Guid GraphExecutionId { get; set; }
        public T GraphValue { get; set; }
        public GraphProgressType EventType { get; set; }
        public string GraphObjectName { get; set; }
        public GraphRunState<T> GraphRunState { get; set; }
    }

    public class GraphReportResult
    {
        public Guid GraphExecutionId { get; set; }
        public Guid CurrentCheckpoint { get; set; }
    }
}
