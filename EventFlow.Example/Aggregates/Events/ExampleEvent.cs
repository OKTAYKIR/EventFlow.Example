using EventFlow.Aggregates;
using EventFlow.EventStores;

namespace EventFlowExample.Aggregates.Events
{
    [EventVersion("example", 1)]
    public class ExampleEvent : AggregateEvent<ExampleAggregate, WizloId>
    {
        public ExampleEvent(int magicNumber)
        {
            MagicNumber = magicNumber;
        }

        public int MagicNumber { get; }
    }
}