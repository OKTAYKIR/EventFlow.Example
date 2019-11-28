using EventFlow.Sagas;
using EventFlow.ValueObjects;

namespace EventFlowExample.Aggregates.Sagas
{
    public class ExampleSagaId : SingleValueObject<string>, ISagaId
    {
        public ExampleSagaId(string value) : base(value)
        {
        }
    }
}
