using EventFlow;
using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using EventFlow.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventFlowExample
{
    public class LogCommandBus : ICommandBus
    {
        private readonly ICommandBus _internalCommandBus;

        public LogCommandBus(ICommandBus commandBus)
        {
            _internalCommandBus = commandBus;
        }

        public Task<TExecutionResult> PublishAsync<TAggregate, TIdentity, TExecutionResult>(ICommand<TAggregate, TIdentity, TExecutionResult> command, CancellationToken cancellationToken)
            where TAggregate : IAggregateRoot<TIdentity>
            where TIdentity : IIdentity
            where TExecutionResult : IExecutionResult
        {
            Console.WriteLine("**********" + command.AggregateId.ToString());
            return _internalCommandBus.PublishAsync<TAggregate, TIdentity, TExecutionResult>(command, cancellationToken);
        }
    }
}
