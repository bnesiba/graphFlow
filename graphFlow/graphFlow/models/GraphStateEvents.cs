using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphFlow.models
{
    public class GraphStateEvent
    {
        public DateTime EventTime { get; set; }
        public Guid GraphStateId { get; set; }

        public GraphStateEvent(Guid graphStateId, DateTime? eventTime = null)
        {
            EventTime = eventTime ?? DateTime.Now;
            GraphStateId = graphStateId;
        }
    }

    public class NodeStart<T> : GraphStateEvent
    {
        public T Input { get; set; }
        public string Name { get; set; }
        public Guid NodeId { get; set; }

        public NodeStart(T input, string name, Guid nodeId, Guid graphStateId, DateTime? eventTime = null) : base(graphStateId, eventTime)
        {
            Input = input;
            Name = name;
            NodeId = nodeId;
        }
    }

    public class NodeComplete<T> : GraphStateEvent
    {
        public T Output { get; set; }
        public string Name { get; set; }
        public Guid NodeId { get; set; }
        public bool Succeeded { get; set; }

        public NodeComplete(T output, string name, Guid nodeId, bool succeeded, Guid graphStateId, DateTime? eventTime = null) : base(graphStateId, eventTime)
        {
            Output = output;
            Name = name;
            NodeId = nodeId;
            Succeeded = succeeded;
        }
    }

    public class EdgeEvaluation : GraphStateEvent
    {
        public Guid TargetNode { get; set; }
        public bool Continue { get; set; }

        public EdgeEvaluation(Guid targetNode, bool shouldContinue, Guid graphStateId, DateTime? eventTime = null) : base(graphStateId, eventTime)
        {
            TargetNode = targetNode;
            Continue = shouldContinue;
        }
    }


}
