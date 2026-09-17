using GraphFlow.persistence.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphFlow.persistence.unSet
{
    public class NotImplementedFlowPersistence<T> : IGraphFlowPersistence<T>
    {
        public StoredGraphExecutionData<T> RetrieveGraphSnapshot(Guid runId)
        {
            //TODO: throw if missing? return null?
            return new StoredGraphExecutionData<T>();
        }

        public GraphThread<T> RetrieveGraphThread(Guid threadId)
        {
            return new GraphThread<T>(Guid.NewGuid(), new List<StoredGraphExecutionData<T>>());
        }

        public void StoreGraphSnapshot(GraphExecutionData<T> persistenceRecord)
        {

        }
    }

}
