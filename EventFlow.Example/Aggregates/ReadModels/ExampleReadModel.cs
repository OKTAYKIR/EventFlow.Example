// The MIT License (MIT)
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using EventFlowExample.Aggregates.Events;
using System.Collections.Generic;

namespace EventFlowExample.Aggregates.ReadModels
{
    public class ExampleReadModel :
        IReadModel,
        IAmReadModelFor<ExampleAggregate, ExampleId, ExampleEvent>
    {
        public List<int> MagicNumber { get; private set; } = new List<int>();

        public void Apply(
            IReadModelContext context,
            IDomainEvent<ExampleAggregate, ExampleId, ExampleEvent> domainEvent)
        {
            MagicNumber.Add(domainEvent.AggregateEvent.MagicNumber);
        }
    }
}