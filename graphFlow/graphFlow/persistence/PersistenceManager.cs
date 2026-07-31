using GraphFlow.persistence.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphFlow.persistence
{
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
            GraphRun<T>? lastRun = thread.Runs.LastOrDefault();
            if (lastRun != null)
            {
                stateObject = lastRun.StateObject;
            }
            return stateObject;
        }

        public T? GetRunState(Guid runId)
        {
            return default(T);
        }
    }
}
