using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphFlow.persistence.models
{
    public class GraphThread<T>
    {
        public Guid ThreadId { get; init; }
        public List<StoredGraphRun<T>> Runs { get; init; }
        //public DateTime CreatedDate { get; init; }//TODO: these can probably just be a getter.
        //public DateTime LastUpdated { get; init; } //TODO: these can probably just be a getter.

        private int runsort(GraphRun<T> run1, GraphRun<T> run2)
        {

            return DateTime.Compare(run1.StartTime, run2.StartTime);
        }

        //private int lastUpdateSort(GraphRun<T> run1, GraphRun<T> run2)
        //{
        //    return DateTime.Compare(run1.EndTime, run2.EndTime);
        //}
        public GraphThread(Guid threadId, List<StoredGraphRun<T>> runs)
        {
            //TODO: this seems inefficient - maybe remove it and do linear threads with forks (runs would have a nullable forkId or something?)
            //Because you *can* have overlapping runs, the last updated run might not be the last run started. 
            //runs.Sort(lastUpdateSort);
            //LastUpdated = runs.FirstOrDefault()?.EndTime ?? DateTime.MinValue;
            runs.Sort(runsort);
            //CreatedDate = runs.FirstOrDefault()?.StartTime ?? DateTime.MinValue;

            ThreadId = threadId;
            Runs = runs;
        }
    }
}
