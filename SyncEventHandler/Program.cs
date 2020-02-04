using EventFlow;
using EventFlow.Aggregates;
using EventFlow.Configuration;
using EventFlow.EventStores;
using EventFlow.Extensions;
using EventFlow.Hangfire.Extensions;
using EventFlow.RabbitMQ;
using EventFlow.RabbitMQ.Integrations;
using EventFlow.Subscribers;
using EventFlowExample.Aggregates;
using EventFlowExample.Aggregates.Events;
using EventFlowExample.Jobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SyncEventHandler
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
               .ConfigureAppConfiguration((host, config) =>
               {
                   config.SetBasePath(Directory.GetCurrentDirectory());
               })
               .ConfigureServices(
                   (hostcontext, services) =>
                   {
                       EventFlowOptions.New
                            .UseHangfireJobScheduler()
                            .AddJobs(typeof(ExampleJob));
                            //.Configure(cfg => cfg.IsAsynchronousSubscribersEnabled = true)
                            //.PublishToRabbitMq(RabbitMqConfiguration.With(new Uri(@"amqp://test:test@localhost:5672"), true, 4, "eventflow"))
                            //.AddAsynchronousSubscriber<ExampleAggregate, WizloId, ExampleEvent, RabbitMqConsumePersistanceService>()
                            //.RegisterServices(s =>
                            //{
                            //    s.Register<IHostedService, RabbitConsumePersistenceService>(Lifetime.Singleton);
                            //    s.Register<IHostedService, RabbitMqConsumePersistanceService>(Lifetime.Singleton);
                            //});
                   })
               .ConfigureLogging((hostingContext, logging) => { });

            await builder.RunConsoleAsync();

            Console.ReadLine();
        }
    }
}
