using ActionFlow.Models;
using graphFlow.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graphFlow
{
    //TODO: maybe selectors don't need to exist?
    public static class GraphFlowSelectors<T>
    {
        public static FlowDataSelector<GraphState<T>, T> GetStateData = new FlowDataSelector<GraphState<T>, T>(GetStateObject);

        public static FlowDataSelector<GraphState<T>, GraphState<T>> GetGraphState = new FlowDataSelector<GraphState<T>, GraphState<T>>(GetState);

        private static T GetState<T>(T state)
        {
            return state;
        }

        private static T GetStateObject<T>(GraphState<T> graphState)
        {
            return graphState.stateObject;
        }
    }
}
