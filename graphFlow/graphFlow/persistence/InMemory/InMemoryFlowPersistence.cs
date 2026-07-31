
using GraphFlow.persistence.models;

namespace GraphFlow.persistence.InMemory
{
    public class InMemoryFlowPersistence<T> : IGraphFlowPersistence<T>
    {
        private Dictionary<Guid, StoredGraphRun<T>> runStorage;
        private Dictionary<Guid, HashSet<Guid>> threadStorage;


        public InMemoryFlowPersistence()
        {
            runStorage = new Dictionary<Guid, StoredGraphRun<T>>();
            threadStorage = new Dictionary<Guid, HashSet<Guid>>();
        }

        public StoredGraphRun<T> RetrieveGraphSnapshot(Guid runId)
        {
            //TODO: throw if missing? return null?
            return runStorage.TryGetValue(runId, out StoredGraphRun<T>? value) ? value : new StoredGraphRun<T>();
        }

        public GraphThread<T> RetrieveGraphThread(Guid threadId)
        {
            var threadRecords = new List<StoredGraphRun<T>>();
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

        public void StoreGraphSnapshot(GraphRun<T> persistenceRecord)
        {
            var id = persistenceRecord.GraphData.id;
            var threadId = persistenceRecord.GraphData.threadId;

            if (!threadStorage.ContainsKey(threadId))
            {
                threadStorage[threadId] = new HashSet<Guid>();
            }
            runStorage[id] = StoredGraphRun<T>.FromGraphRun(persistenceRecord);
            threadStorage[threadId].Add(id);
        }
    }
}
