using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using EventFlowExample.Aggregates.Commands;
using EventFlowExample.Aggregates.Events;

namespace EventFlowExample.Aggregates.CommandHandlers
{
    /// Command handler for our command
    public class ExampleCommandHandler :
        CommandHandler<ExampleAggregate, WizloId, IExecutionResult, ExampleCommand>
    {
        public override Task<IExecutionResult> ExecuteCommandAsync(
            ExampleAggregate aggregate,
            ExampleCommand command,
            CancellationToken cancellationToken)
        {
            var executionResult = aggregate.SetMagicNumer(command.MagicNumber);
            return Task.FromResult(executionResult);
        }
    }
}