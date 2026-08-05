using GraphFlow.persistence.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GraphFlow.persistence
{
    //TODO: does this still need to exist?
    public class PersistenceManager<T>
    {
        public IGraphFlowPersistence<T> _graphPersistence;
        public PersistenceManager(IGraphFlowPersistence<T> graphFlowPersistence) 
        {
            _graphPersistence = graphFlowPersistence;
        }

        public T? GetLatestThreadState(Guid threadId)
        {
            T? stateObject = default(T);
            var thread = _graphPersistence.RetrieveGraphThread(threadId);
            StoredGraphRun<T>? lastRun = thread.Runs.LastOrDefault();
            if (lastRun != null)
            {
                stateObject = lastRun.StateObject;
            }
            return stateObject;
        }

        public T? GetRunState(Guid runId)
        {
            T? stateObject = default(T);
            StoredGraphRun<T>? run = _graphPersistence.RetrieveGraphSnapshot(runId);
            if (run != null)
            {
                stateObject = run.StateObject;
            }
            return stateObject;
        }

        public StoredGraphRun<T> GetRun(Guid runId)
        {
            StoredGraphRun<T> runResponse = new StoredGraphRun<T>(); 
            StoredGraphRun<T>? run = _graphPersistence.RetrieveGraphSnapshot(runId);
            if(run != null)
            {
                runResponse = run;
            }
            return runResponse;
        }

        public GraphThread<T> GetThread(Guid threadId)
        {
            //TODO: (for all of these) should this error when not found?
            GraphThread<T> graphThread = new GraphThread<T>(Guid.Empty, new List<StoredGraphRun<T>>()); 
            GraphThread<T>? foundThread = _graphPersistence.RetrieveGraphThread(threadId);
            if (foundThread != null)
            {
                graphThread = foundThread;
            }
            return graphThread;
        }

        //TODO: get all?
    }
}
