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
    public class GraphState<T>: GraphStateBase
    {
        public GraphState(T state) : base()
        {
            stateObject = state;
        }

        public GraphState(): base()
        {
            stateObject = default(T);
        }

        public T stateObject {  get; set; }

    }

    public class GraphState : GraphStateBase
    {
        public GraphState() : base() { }
    }

    public abstract class GraphStateBase
    {
        public Guid id {  get; set; }
        public List<GraphStateEvent> graphStateEvents { get; set; }

        public GraphStateBase()
        {
            id = Guid.NewGuid();
            graphStateEvents = new List<GraphStateEvent>();
        }
    }
}
