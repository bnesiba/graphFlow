using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graphFlow.models
{
    public class GraphState<T> : GraphStateBase
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
        public List<string> nodesAndEdgesRun { get; set; }

        public GraphStateBase()
        {
            id = Guid.NewGuid();
            nodesAndEdgesRun = new List<string>();
        }
    }
}
