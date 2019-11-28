using EventFlow.Aggregates;
using EventFlow.EventStores;

namespace EventFlowExample.Aggregates.Sagas.Events
{
    [EventVersion("example_saga_completed", 1)]
    public class ExampleSagaCompletedEvent : AggregateEvent<ExampleSaga, ExampleSagaId>
    {
        public ExampleSagaCompletedEvent()
        {
        }
    }
}