using ActionFlow;
using GraphFlow.models;
using GraphFlow.flow;
using GraphFlow.persistence;
using GraphFlow.persistence.unSet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GraphFlow.util
{
    public static class GraphFlowServiceCollection
    {
        public static IServiceCollection UseGraphFlow<T>(this IServiceCollection services)
            where T : class, IDefaultValueHaver<T>
        {
            services.UseFlowState();
            services.UseEffects<GraphFlowEffects<T>>();
            services.UseReducer<StateObjectReducer<T>, T>();
            services.UseReducer<GraphRunStateReducer<T>, GraphRunState<T>>();
            services.AddSingleton<IGraphFlowPersistence<T>, NotImplementedFlowPersistence<T>>();
            services.AddScoped<GraphBuilder<T>>();
            services.AddScoped<PersistenceManager<T>>();
            return services;
        }


        public static IServiceCollection UseInMemoryPersistence<T>(this IServiceCollection services)
        {
            services.RemoveAll<IGraphFlowPersistence<T>>();
            services.UseEffects<GraphPersistenceEffects<T>>();
            return services;
        }


    }
}
