using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using EventFlowExample.Aggregates.Commands;
using EventFlowExample.Aggregates.Events;

namespace EventFlowExample.Aggregates.CommandHandlers
{
    public class ResetCommandHandler : CommandHandler<ExampleAggregate, WizloId, IExecutionResult, ResetCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(
            ExampleAggregate aggregate,
            ResetCommand command,
            CancellationToken cancellationToken)
        {
            var executionResult = aggregate.ResetMagicNumber();

            return Task.FromResult(executionResult);
        }
    }
}