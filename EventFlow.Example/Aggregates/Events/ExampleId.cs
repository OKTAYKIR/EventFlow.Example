using EventFlow.Core;

namespace EventFlowExample.Aggregates.Events
{
    public class ExampleId : IIdentity
    {
        public ExampleId(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}