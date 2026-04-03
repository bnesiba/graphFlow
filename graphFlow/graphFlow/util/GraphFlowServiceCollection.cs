using ActionFlow;
using graphFlow.models;
using GraphFlow.flow;
using Microsoft.Extensions.DependencyInjection;

namespace graphFlow.util
{
    public static class GraphFlowServiceCollection
    {
        public static IServiceCollection UseGraphFlow<T>(this IServiceCollection services)
            where T : class, IDefaultValueHaver<T>
        {
            services.UseFlowState();
            services.UseEffects<GraphFlowEffects<T>>();
            services.UseReducer<StateObjectReducer<T>, T>();
            services.UseReducer<GraphFlowReducer<T>, GraphState<T>>();
            services.AddScoped<GraphBuilder<T>>();
            return services;
        }
    }
}
