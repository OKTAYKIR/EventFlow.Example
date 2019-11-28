using EventFlow.Aggregates;
using EventFlow.Sagas;
using System.Threading;
using System.Threading.Tasks;

namespace EventFlowExample.Aggregates.Sagas
{
    public class OrderSagaLocator : ISagaLocator
    {
        public Task<ISagaId> LocateSagaAsync(
          IDomainEvent domainEvent,
          CancellationToken cancellationToken)
        {
            //TODO:
            var event_version = domainEvent.Metadata["event_version"];
            var orderSagaId = new ExampleSagaId($"saga-{event_version}");

            return Task.FromResult<ISagaId>(orderSagaId);
        }
    }
}
