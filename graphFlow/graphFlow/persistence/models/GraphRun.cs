using graphFlow.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphFlow.persistence.models
{
    public class GraphRun<T>
    {
        public T StateObject { get; set; }
        public GraphState<T> GraphData { get; set; }
        public DateTime StartTime { get { return GraphData.graphStateEvents.FirstOrDefault()?.EventTime ?? DateTime.MinValue; } }
        public DateTime EndTime { get { return GraphData.graphStateEvents.LastOrDefault()?.EventTime ?? DateTime.MinValue; } }

    }

    public class StoredGraphRun<T>: GraphRun<T>
    {
        public DateTime recordDate { get; set; }

        public static StoredGraphRun<T> FromGraphRun(GraphRun<T> record)
        {
            return new StoredGraphRun<T>
            {
                GraphData = record.GraphData,
                StateObject = record.StateObject,
                recordDate = DateTime.Now,
            };
        }
    }
}
