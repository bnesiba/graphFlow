using ActionFlow;
using graphFlow.models;
using graphFlow.util;
using GraphFlow.flow;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphFlow.persistence.InMemory
{
    public static class InMemoryPersistenceServiceCollection
    {
        public static IServiceCollection UseInMemoryPersistence<T>(this IServiceCollection services)
            where T : class, IDefaultValueHaver<T>
        {
            services.AddSingleton<InMemoryFlowPersistence<T>>();
            services.UseEffects<GraphPersistenceEffects<T>>();
            return services;
        }
    }
}
