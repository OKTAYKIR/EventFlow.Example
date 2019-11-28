using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;

namespace EventFlowExample.Models
{
    public class CommandReturnResult : ExecutionResult
    {
        public CommandReturnResult(bool isSuccess, IAggregateRoot aggregateRoot)
        {
            IsSuccess = isSuccess;
            AggregateRoot = aggregateRoot;
        }

        public override bool IsSuccess { get; }
        public IAggregateRoot AggregateRoot { get; }
    }
}
