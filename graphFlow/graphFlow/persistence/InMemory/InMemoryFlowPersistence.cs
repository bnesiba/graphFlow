
namespace GraphFlow.persistence.InMemory
{
    public class InMemoryFlowPersistence<T> : IGraphFlowPersistence<T>
    {
        private Dictionary<Guid, StoredPersistenceRecord<T>> stateStorage;
        private Dictionary<Guid, HashSet<Guid>> threadStorage;


        public InMemoryFlowPersistence()
        {
            stateStorage = new Dictionary<Guid, StoredPersistenceRecord<T>>();
            threadStorage = new Dictionary<Guid, HashSet<Guid>>();
        }

        public StoredPersistenceRecord<T> RetrieveGraphSnapshot(Guid id)
        {
            //TODO: throw if missing? return null?
            return stateStorage.TryGetValue(id, out StoredPersistenceRecord<T>? value) ? value : new StoredPersistenceRecord<T>();
        }

        public List<StoredPersistenceRecord<T>> RetrieveGraphThread(Guid threadId)
        {
            var threadRecords = new List<StoredPersistenceRecord<T>>();
            if(threadStorage.TryGetValue(threadId, out var thread))
            {
                foreach (var recordId in thread)
                {
                    if(stateStorage.TryGetValue(recordId, out var record))
                    {
                        threadRecords.Add(record);
                    }
                    else
                    {
                        //TODO: throw or something? this shouldn't happen.
                    }
                }
            }
            return threadRecords;
        }

        public void StoreGraphSnapshot(PersistenceRecord<T> persistenceRecord)
        {
            var id = persistenceRecord.graphState.id;
            var threadId = persistenceRecord.graphState.threadId;

            if (!threadStorage.ContainsKey(threadId))
            {
                threadStorage[threadId] = new HashSet<Guid>();
            }
            stateStorage[id] = StoredPersistenceRecord<T>.FromPersistanceRecord(persistenceRecord);
            threadStorage[threadId].Add(id);
        }
    }
}
