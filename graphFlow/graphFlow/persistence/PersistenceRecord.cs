using graphFlow.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphFlow.persistence
{
    public class PersistenceRecord<T>
    {
        public T stateObject { get; set; }
        public GraphState<T> graphState { get; set; }
    }

    public class StoredPersistenceRecord<T>: PersistenceRecord<T>
    {
        public DateTime recordDate { get; set; }

        public static StoredPersistenceRecord<T> FromPersistanceRecord(PersistenceRecord<T> record)
        {
            return new StoredPersistenceRecord<T>
            {
                graphState = record.graphState,
                stateObject = record.stateObject,
                recordDate = DateTime.Now,
            };
        }
    }
}
