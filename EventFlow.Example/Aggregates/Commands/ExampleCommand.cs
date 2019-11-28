using EventFlow.Commands;
using EventFlowExample.Aggregates.Events;
using EventFlowExample.Models;

namespace EventFlowExample.Aggregates.Commands
{
    public class ExampleCommand : Command<ExampleAggregate, WizloId, CommandReturnResult>
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