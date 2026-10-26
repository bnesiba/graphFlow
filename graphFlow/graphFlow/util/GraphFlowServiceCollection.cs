using ActionFlow;
using GraphFlow.flow;
using GraphFlow.models;
using GraphFlow.persistence;
using GraphFlow.persistence.InMemory;
using GraphFlow.persistence.unSet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GraphFlow.util
{
    public static class GraphFlowServiceCollection
    {
        public static IServiceCollection UseGraphFlow<T>(this IServiceCollection services)
            where T : class
        {
            services.UseFlowState();
            services.UseEffects<GraphFlowEffects<T>>();
            services.UseReducer<StateObjectReducer<T>, GraphFlowState<T>>();
            services.UseReducer<GraphRunStateReducer<T>, GraphOrchestrationState<T>>();
            services.AddSingleton<IGraphFlowPersistence<T>, NotImplementedFlowPersistence<T>>();
            services.AddScoped<GraphBuilder<T>>();
            services.AddScoped<PersistenceManager<T>>();
            return services;
        }


        public static IServiceCollection UseInMemoryPersistence<T>(this IServiceCollection services)
        {
            services.RemoveAll<IGraphFlowPersistence<T>>();
            services.AddSingleton<IGraphFlowPersistence<T>, InMemoryFlowPersistence<T>>();
            services.UseEffects<GraphPersistenceEffects<T>>();
            return services;
        }


    }
}
