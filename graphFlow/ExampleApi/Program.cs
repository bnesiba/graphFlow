
using ActionFlow;
using ExampleApi.Models;
using graphFlow;
using graphFlow.models;

namespace ExampleApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //TODO: flow config goes here
            builder.Services.UseFlowState();
            builder.Services.UseEffects<GraphFlowEffects<ExampleGraphStateObject>>();
            builder.Services.UseReducer<StateObjectReducer<ExampleGraphStateObject>, ExampleGraphStateObject>();
            //builder.Services.UseReducer<GraphFlowReducer<ExampleGraphStateObject>, GraphState<ExampleGraphStateObject>>();

            //local flow config?
            builder.Services.AddScoped<FlowGraph>();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
