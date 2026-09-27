using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GraphFlow.checkpointing
{
    //TODO: should other checkpointing/graphstate stuff be here?
    internal sealed class GraphRunSerializer<T>
    {
        private readonly JsonSerializerOptions _options;

        public GraphRunSerializer(JsonSerializerOptions? options = null)
        {
            _options = options ?? new JsonSerializerOptions();
            _options.PropertyNameCaseInsensitive = true;
            _options.WriteIndented = true;//TODO: doesn't need indentation - just makes debugging easier for now
        }

        public string SerializeGraphRun(T graphRun)
        {
            string serialized = JsonSerializer.Serialize(graphRun, _options);
            return serialized;
        }
    }
}
