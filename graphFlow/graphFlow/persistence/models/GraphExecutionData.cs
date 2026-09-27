using GraphFlow.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphFlow.persistence.models
{
    public class GraphExecutionData<T>
    {
        public T StateObject { get; set; }
        public GraphRunState<T> GraphData { get; set; }
        public DateTime StartTime { get { return GraphData.GraphEvents.FirstOrDefault()?.EventTime ?? DateTime.MinValue; } }
        public DateTime EndTime { get { return GraphData.GraphEvents.LastOrDefault()?.EventTime ?? DateTime.MinValue; } }

    }

    public class StoredGraphExecutionData<T>: GraphExecutionData<T>
    {
        public DateTime recordDate { get; set; }

        public static StoredGraphExecutionData<T> FromGraphRun(GraphExecutionData<T> record)
        {
            return new StoredGraphExecutionData<T>
            {
                GraphData = record.GraphData,
                StateObject = record.StateObject,
                recordDate = DateTime.Now,
            };
        }
    }
}
