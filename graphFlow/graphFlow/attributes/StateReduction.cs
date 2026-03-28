using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//TODO: remove?
namespace graphFlow.attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class StateReducerAttribute : Attribute
    {
        public Type ReducerType { get; init; }

        public StateReducerAttribute(Type reducerType)
        {
            ReducerType = reducerType;
        }
    }

    //TODO: move if useful - otherwise remove
    public interface IGraphReducer<T>
    {
        T Reduce(T OldValue, T NewValue);
    }

    public sealed class OverWriteReducer : IGraphReducer<object>
    {
        public object Reduce(object OldValue, object NewValue)
        {
            return NewValue;
        }
    }

    public sealed class ListMergeReducer<T> : IGraphReducer<List<T>>
    {
        public List<T> Reduce(List<T> OldValue, List<T> NewValue)
        {
            OldValue.AddRange(NewValue);
            return OldValue;
        }
    }
}
