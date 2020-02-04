using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using EventFlow.Subscribers;
using EventFlowExample.Aggregates.Events;

namespace EventFlowExample.Aggregates.Subscribers
{
    public class ExampleSyncSubscriber : ISubscribeSynchronousTo<ExampleAggregate, ExampleId, ExampleEvent>
    {
        public Task HandleAsync(IDomainEvent<ExampleAggregate, ExampleId, ExampleEvent> domainEvent, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            { 
                Console.WriteLine("++++++++" + domainEvent.ToString());
            });
        }            
    }
}