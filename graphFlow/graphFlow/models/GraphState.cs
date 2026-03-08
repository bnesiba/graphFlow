using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graphFlow.models
{
    public class GraphState<T> : GraphStateBase
    {
        public T stateObject {  get; set; }
    }

    public class GraphState : GraphStateBase
    {
    }

    public abstract class GraphStateBase
    {
        public Guid id {  get; set; }
        public List<string> nodesAndEdgesRun { get; set; }
    }
}
