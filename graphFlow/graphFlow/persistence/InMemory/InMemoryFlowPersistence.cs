
using GraphFlow.persistence.models;

namespace GraphFlow.persistence.InMemory
{
    public class InMemoryFlowPersistence<T> : IGraphFlowPersistence<T>
    {
        private Dictionary<Guid, StoredGraphExecutionData<T>> runStorage;
        private Dictionary<Guid, HashSet<Guid>> threadStorage;


        public InMemoryFlowPersistence()
        {
            runStorage = new Dictionary<Guid, StoredGraphExecutionData<T>>();
            threadStorage = new Dictionary<Guid, HashSet<Guid>>();
        }

        public StoredGraphExecutionData<T> RetrieveGraphSnapshot(Guid runId)
        {
            //TODO: throw if missing? return null?
            return runStorage.TryGetValue(runId, out StoredGraphExecutionData<T>? value) ? value : new StoredGraphExecutionData<T>();
        }

        public GraphThread<T> RetrieveGraphThread(Guid threadId)
        {
            var threadRecords = new List<StoredGraphExecutionData<T>>();
            if(threadStorage.TryGetValue(threadId, out var thread))
            {
                foreach (var recordId in thread)
                {
                    if(runStorage.TryGetValue(recordId, out var record))
                    {
                        threadRecords.Add(record);
                    }
                    else
                    {
                        //TODO: throw or something? this shouldn't happen.
                    }
                }
            }
            var graphThread = new GraphThread<T>(threadId, threadRecords);
            return graphThread;
        }

        public void StoreGraphSnapshot(GraphExecutionData<T> persistenceRecord)
        {
            var id = persistenceRecord.GraphData.Id;
            var threadId = persistenceRecord.GraphData.ThreadId;

            if (!threadStorage.ContainsKey(threadId))
            {
                threadStorage[threadId] = new HashSet<Guid>();
            }
            runStorage[id] = StoredGraphExecutionData<T>.FromGraphRun(persistenceRecord);
            threadStorage[threadId].Add(id);
        }
    }
}
