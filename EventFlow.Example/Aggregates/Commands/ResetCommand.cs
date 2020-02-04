using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using EventFlowExample.Aggregates.Events;

namespace EventFlowExample.Aggregates.Commands
{
    public class ResetCommand : Command<ExampleAggregate, ExampleId, IExecutionResult>
    {
        public ResetCommand(
            ExampleId aggregateId)
            : base(aggregateId
            )
        {
        }
    }
}