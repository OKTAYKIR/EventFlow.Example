using EventFlow.Configuration;
using EventFlow.Jobs;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventFlowExample.Jobs
{
    [JobVersion("ExampleJob", 1)]
    public class ExampleJob : IJob
    {
        public ExampleJob(string message)
        {
            Message = message;
        }

        public string Message { get; }

        public Task ExecuteAsync(
          IResolver resolver,
          CancellationToken cancellationToken)
        {
            Console.WriteLine(Message);

            return Task.CompletedTask;
        }
    }
}
