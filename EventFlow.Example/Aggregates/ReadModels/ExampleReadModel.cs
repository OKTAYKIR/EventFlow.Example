// The MIT License (MIT)
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using EventFlowExample.Aggregates.Events;
using System.Collections.Generic;

namespace EventFlowExample.Aggregates.ReadModels
{
    /// Read model for our aggregate
    public class ExampleReadModel :
        IReadModel,
        IAmReadModelFor<ExampleAggregate, WizloId, ExampleEvent>
    {
        public List<int> MagicNumber { get; private set; } = new List<int>();

        public void Apply(
            IReadModelContext context,
            IDomainEvent<ExampleAggregate, WizloId, ExampleEvent> domainEvent)
        {
            MagicNumber.Add(domainEvent.AggregateEvent.MagicNumber);
        }
    }
}