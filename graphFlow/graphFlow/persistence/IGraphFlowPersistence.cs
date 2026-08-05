using graphFlow.models;
using GraphFlow.persistence.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphFlow.persistence
{
    public interface IGraphFlowPersistence<T>
    {
        public void StoreGraphSnapshot( GraphRun<T> persistenceRecord);

        public StoredGraphRun<T> RetrieveGraphSnapshot(Guid runId);

        public GraphThread<T> RetrieveGraphThread(Guid threadId);

        //TODO: search?
        //TODO: get all? get by timespan? 
    }
}
