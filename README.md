# GraphFlow

Build agent loops and other stateful workflows in C# on .NET 8. A node does one piece of work and returns the updated state. An edge reads that state and decides where execution goes next. That gives you a place to answer a familiar agent question in code: **do we have enough information to answer, or should we call a tool first?**

GraphFlow uses [ActionFlow](https://www.nuget.org/packages/bcn.ActionFlow/) actions, reducers, and effects to drive execution and track state. For model-provider abstractions and agent examples, see [AgentFlow](https://github.com/bnesiba/agentflow), a related project that builds on GraphFlow.

## Try a simple branch

A score of 60 or higher passes. Anything lower fails. Here’s the whole graph, starting with a `GraphBuilder<Grade>` named `builder`:

```csharp
var graph = builder.GetExecutableGraph();

graph.AddNode("Check score", grade => grade);
graph.AddNode("Pass", grade => grade with { Result = "Pass" });
graph.AddNode("Fail", grade => grade with { Result = "Fail" });

graph.SetStartNode("Check score");
graph.AddEdge("Check score", "Pass", grade => grade.Score >= 60);
graph.AddEdge("Check score", "Fail");

var run = graph.ExecuteGraph(new Grade(75));
Console.WriteLine(run.StateObject.Result); // Pass

public record Grade(int Score, string Result = "");
```

GraphFlow checks outgoing edges in the order you add them. A score of 75 takes the first edge. Try 59 and the graph takes the unconditional edge to `Fail`. Both routes end after setting the result because neither destination has an outgoing edge.

To use this in a .NET 8 app, install the [GraphFlow NuGet package](https://www.nuget.org/packages/bcn.GraphFlow/) with `dotnet add package bcn.GraphFlow`. Import `GraphFlow.util`, register `services.UseGraphFlow<Grade>()`, and inject `GraphBuilder<Grade>` into the class that builds the graph. The sample API shows the [service registration](https://github.com/bnesiba/graphFlow/blob/main/graphFlow/ExampleApi/Program.cs) and [constructor injection](https://github.com/bnesiba/graphFlow/blob/main/graphFlow/ExampleApi/ExampleGraph.cs).

## Watch a run

`ExecuteGraph` returns your final state in `StateObject` and the execution history in `GraphData.GraphEvents`. To see progress during execution, replace the `ExecuteGraph` call above with `StreamGraph`. The callback receives each checkpoint, and the method returns the same result type.

```csharp
var run = graph.StreamGraph(new Grade(75), progress =>
{
    Console.WriteLine($"{progress.GraphObjectName}: {progress.GraphRunState.CurrentCheckpoint}");
});
```

## Run the repository example

From the repository root, start the included ASP.NET Core API.

```sh
dotnet run --project graphFlow/ExampleApi/ExampleApi.csproj --launch-profile http
```

Open [Swagger UI](http://localhost:5146/swagger) or try the endpoints directly. The first two run the same graph with different starting state.

| Request | What to look for |
| --- | --- |
| [GET /api/GraphExample/test](http://localhost:5146/api/GraphExample/test) | The short route visits nodes 1, 2, and 3. The final state's `nodeCount` is 3. |
| [GET /api/GraphExample/testWithValue](http://localhost:5146/api/GraphExample/testWithValue) | Starting with the flag set takes the route 1 → 2 → 4 → 5 → 1 → 2 → 3. The final count is 7. |
| [GET /api/GraphExample/testWithStreaming](http://localhost:5146/api/GraphExample/testWithStreaming) | The response includes the checkpoints collected during the run alongside the final result. |
| [GET /api/GraphExample/testWithParallel](http://localhost:5146/api/GraphExample/testWithParallel) | The controller runs the same graph three times in parallel and returns the three final states. |

Read the [graph definition and routing predicates](https://github.com/bnesiba/graphFlow/blob/main/graphFlow/ExampleApi/ExampleGraph.cs), the [state model](https://github.com/bnesiba/graphFlow/blob/main/graphFlow/ExampleApi/Models/ExampleGraphState.cs), and the [controller that executes and streams the graph](https://github.com/bnesiba/graphFlow/blob/main/graphFlow/ExampleApi/Controllers/GraphExampleController.cs).
