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
        public StoredGraphRun<T> RetrieveGraphSnapshot(Guid runId)
        {
            //TODO: throw if missing? return null?
            return new StoredGraphRun<T>();
        }

        public GraphThread<T> RetrieveGraphThread(Guid threadId)
        {
            return new GraphThread<T>(Guid.NewGuid(), new List<StoredGraphRun<T>>());
        }

        public void StoreGraphSnapshot(GraphRun<T> persistenceRecord)
        {

        }
    }

}
