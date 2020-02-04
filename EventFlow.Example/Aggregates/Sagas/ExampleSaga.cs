using EventFlow.Aggregates;
using EventFlow.Exceptions;
using EventFlow.Sagas;
using EventFlow.Sagas.AggregateSagas;
using EventFlowExample.Aggregates.Commands;
using EventFlowExample.Aggregates.Events;
using EventFlowExample.Aggregates.Sagas.Events;
using System.Threading;
using System.Threading.Tasks;

namespace EventFlowExample.Aggregates.Sagas
{
    public class ExampleSaga : AggregateSaga<ExampleSaga, ExampleSagaId, OrderSagaLocator>,
                               ISagaIsStartedBy<ExampleAggregate, ExampleId, ExampleEvent>,
                               ISagaHandles<ExampleAggregate, ExampleId, ResetEvent>,
                               IEmit<ExampleSagaStartedEvent>
    {
         int MagicNumber { get; set; }
       
         public ExampleSaga(ExampleSagaId id) : base(id)
         {
         }
       
         public Task HandleAsync(
             IDomainEvent<ExampleAggregate, ExampleId, ExampleEvent> domainEvent,
             ISagaContext sagaContext,
             CancellationToken cancellationToken)
         {
             // This check is redundant! We do it to verify EventFlow works correctly
             if (State != SagaState.New) 
                 throw DomainError.With("Saga must be new!");
       
             Emit(new ExampleSagaStartedEvent(domainEvent.AggregateEvent.MagicNumber));
       
             Publish(new ResetCommand(domainEvent.AggregateIdentity));
       
             return Task.CompletedTask;
         }
       
         public Task HandleAsync(
            IDomainEvent<ExampleAggregate, ExampleId, ResetEvent> domainEvent,
            ISagaContext sagaContext,
            CancellationToken cancellationToken)
        {
            // This check is redundant! We do it to verify EventFlow works correctly
            if (State != SagaState.Running) throw DomainError.With("Saga must be running!");

            Emit(new ExampleSagaCompletedEvent());

            return Task.FromResult(0);
        }

        public void Apply(ExampleSagaStartedEvent @event)
        {
            // Update our aggregate state with relevant order details
            MagicNumber = @event.MagicNumber;
        }

        public void Apply(ExampleSagaCompletedEvent @event)
        {
            Complete();
        }
    }
}
