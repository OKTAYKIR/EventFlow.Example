using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;
using EventFlowExample.Aggregates.Commands;
using EventFlowExample.Aggregates.Events;
using EventFlowExample.Models;

namespace EventFlowExample.Aggregates.CommandHandlers
{
    public class ExampleCommandHandler :
        CommandHandler<ExampleAggregate, WizloId, CommandReturnResult, ExampleCommand>
    {
        public override Task<CommandReturnResult> ExecuteCommandAsync(
            ExampleAggregate aggregate,
            ExampleCommand command,
            CancellationToken cancellationToken)
        {
            var executionResult = aggregate.SetMagicNumer(command.MagicNumber);

            return Task.FromResult(new CommandReturnResult(executionResult.IsSuccess, aggregate));
        }
    }
}