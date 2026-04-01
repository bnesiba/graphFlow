using graphFlow;
using graphFlow.models;

namespace ExampleApi.Models
{
    public class ExampleGraphStateObject: IDefaultValueHaver<ExampleGraphStateObject>
    {
        public Dictionary<string, string> NodeOutputs { get; set; }
        public int NodeCount { get; set; }
        public bool ShouldDoTheThing { get; set; }

        public static ExampleGraphStateObject DefaultValue()
        {
            return new ExampleGraphStateObject
            {
                NodeCount = 0,
                ShouldDoTheThing = false,
                NodeOutputs = new Dictionary<string, string>()
            };
        }
    }
}
