using EventFlow.Aggregates;
using EventFlow.EventStores;

namespace EventFlowExample.Aggregates.Sagas.Events
{
    [EventVersion("example_saga_started", 1)]
    public class ExampleSagaStartedEvent : AggregateEvent<ExampleSaga, ExampleSagaId>
    {
        public ExampleSagaStartedEvent(int magicNumber)
        {
            MagicNumber = magicNumber;
        }

        public int MagicNumber { get; }
    }
}