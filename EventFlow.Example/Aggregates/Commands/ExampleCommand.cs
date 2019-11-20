using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using EventFlowExample.Aggregates.Events;

namespace EventFlowExample.Aggregates.Commands
{
    /// Command for update magic number
    public class ExampleCommand : Command<ExampleAggregate, WizloId, IExecutionResult>
    {
        public ExampleCommand(
            WizloId aggregateId,
            //ISourceId sourceId, //optional
            int magicNumber)
            : base(aggregateId
                  // ,sourceId //optional
            )
        {
            MagicNumber = magicNumber;
        }

        public int MagicNumber { get; }
    }
}