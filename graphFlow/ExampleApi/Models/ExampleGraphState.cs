using graphFlow.models;

namespace ExampleApi.Models
{
    public class ExampleGraphState : GraphStateObject<ExampleGraphStateObject>
    {
        

        public ExampleGraphState() 
        {
            this.InitialValue = new ExampleGraphStateObject
            {
                NodeOutputs = new Dictionary<string, string>(),
                NodeCount = 0,
                ShouldDoTheThing = false,
                };
        }

        //TODO: implement real reducer - this is unnecessary at the moment.
        //public override ExampleGraphStateObject Reduce(ExampleGraphStateObject oldValue, ExampleGraphStateObject newValue)
        //{
        //    return base.Reduce(oldValue, newValue);
        //}
    }


    public class ExampleGraphStateObject
    {
        public Dictionary<string, string> NodeOutputs { get; set; }
        public int NodeCount { get; set; }
        public bool ShouldDoTheThing { get; set; }
    }
}
