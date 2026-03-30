using ActionFlow.Models;
using graphFlow.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graphFlow
{
    //TODO: update actionflow to make selectors unnecessary?
    public static class StateObjectSelectors<T>
    {
        public static FlowDataSelector<T, T> GetStateData = new FlowDataSelector<T, T>(GetState);


        private static T GetState<T>(T state)
        {
            return state;
        }
    }
}
