using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;
using EventFlow.Jobs;
using EventFlowExample.Aggregates.Commands;
using EventFlowExample.Aggregates.Events;
using EventFlowExample.Jobs;
using EventFlowExample.Models;

namespace EventFlowExample.Aggregates.CommandHandlers
{
    public class ExampleCommandHandler :
        CommandHandler<ExampleAggregate, WizloId, CommandReturnResult, ExampleCommand>
    {
        private readonly IJobScheduler _jobScheduler;

        public ExampleCommandHandler(IJobScheduler jobScheduler)
        {
            _jobScheduler = jobScheduler;
        }

        public override async Task<CommandReturnResult> ExecuteCommandAsync(
            ExampleAggregate aggregate,
            ExampleCommand command,
            CancellationToken cancellationToken)
        {
            var executionResult = aggregate.SetMagicNumer(command.MagicNumber);

            await _jobScheduler
                .ScheduleAsync(new ExampleJob("Example job"), TimeSpan.FromSeconds(10), CancellationToken.None)
                .ConfigureAwait(false);

            return await Task.FromResult(new CommandReturnResult(executionResult.IsSuccess, aggregate));
        }
    }
}