using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using EventFlowExample.Aggregates.Events;

namespace EventFlowExample.Aggregates.Commands
{
    public class ResetCommand : Command<ExampleAggregate, WizloId, IExecutionResult>
    {
        public ResetCommand(
            WizloId aggregateId)
            : base(aggregateId
            )
        {
        }
    }
}