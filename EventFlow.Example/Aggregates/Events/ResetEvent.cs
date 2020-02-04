using EventFlow.Aggregates;
using EventFlow.EventStores;

namespace EventFlowExample.Aggregates.Events
{
    [EventVersion("reset", 1)]
    public class ResetEvent : AggregateEvent<ExampleAggregate, ExampleId>
    {
        public ResetEvent()
        {
        }
    }
}