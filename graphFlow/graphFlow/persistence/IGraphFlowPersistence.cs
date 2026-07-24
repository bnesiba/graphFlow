using graphFlow.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphFlow.persistence
{
    public interface IGraphFlowPersistence<T>
    {
        public void StoreGraphSnapshot( PersistenceRecord<T> persistenceRecord);

        public StoredPersistenceRecord<T> RetrieveGraphSnapshot(Guid snapshotId);

        public List<StoredPersistenceRecord<T>> RetrieveGraphThread(Guid threadId);
    }
}
