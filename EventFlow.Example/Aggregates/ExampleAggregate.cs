using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Snapshots;
using EventFlow.Snapshots.Strategies;
using EventFlowExample.Aggregates.Events;
using EventFlowExample.Aggregates.Snapshots;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EventFlowExample.Aggregates
{
    public class ExampleAggregate : SnapshotAggregateRoot<ExampleAggregate, WizloId, ExampleSnaphost>,
                                    IEmit<ExampleEvent> //optional
    {
        #region Aggregate Variables
        private int? _magicNumber;
        private int _counter = 0;
        #endregion

        public ExampleAggregate(WizloId id) : base(id, SnapshotEveryFewVersionsStrategy.With(SnapshotEveryVersion)) 
        {
        //    SetSourceIdHistory(int.MaxValue); //optional;
        }

        #region Snapshots
        public const int SnapshotEveryVersion = 100;
        public IReadOnlyCollection<ExampleSnapshotVersion> SnapshotVersions { get; private set; } = new ExampleSnapshotVersion[] { };

        protected override Task<ExampleSnaphost> CreateSnapshotAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(new ExampleSnaphost(
                                   _magicNumber,
                                   _counter,
                                   Enumerable.Empty<ExampleSnapshotVersion>()));
        }

        protected override Task LoadSnapshotAsync(ExampleSnaphost snapshot, ISnapshotMetadata metadata, CancellationToken cancellationToken)
        {
            _magicNumber = snapshot.MagicNumber;
            _counter = snapshot.Counter;

            SnapshotVersions = snapshot.PreviousVersions;

            return Task.FromResult(0);
        }
        #endregion

        // Method invoked by our command
        public IExecutionResult SetMagicNumer(int magicNumber)
        {
            Emit(new ExampleEvent(magicNumber));
            
            return ExecutionResult.Success();
        }

        public IExecutionResult ResetMagicNumber()
        {
            Emit(new ResetEvent());

            return ExecutionResult.Success();
        }

        // We apply the event as part of the event sourcing system. EventFlow
        // provides several different methods for doing this, e.g. state objects,
        // the Apply method is merely the simplest
        public void Apply(ExampleEvent aggregateEvent)
        {
            _magicNumber = aggregateEvent.MagicNumber;

            _counter += _magicNumber.Value;
        }

        public void Apply(ResetEvent aggregateEvent)
        {
            _magicNumber = 0;

            _counter = 0;
        }
    }
}