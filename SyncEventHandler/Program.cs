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
    //public class RabbitConsumePersistenceService : IHostedService, IDisposable
    //{
    //    private readonly IDispatchToEventSubscribers _dispatchToEventSubscribers;
    //   // private readonly EnvironmentConfiguration _environmentConfiguration;
    //    private readonly IEventJsonSerializer _eventJsonSerializer;
    //    private readonly IRabbitMqConfiguration _rabbitMqConfiguration;
    //    private readonly IRabbitMqConnectionFactory _rabbitMqConnectionFactory;


    //    public RabbitConsumePersistenceService(
    //     //   EnvironmentConfiguration environmentConfiguration,
    //        IRabbitMqConnectionFactory rabbitMqConnectionFactory,
    //        IRabbitMqConfiguration rabbitMqConfiguration,
    //        IEventJsonSerializer eventJsonSerializer,
    //        IDispatchToEventSubscribers dispatchToEventSubscribers)
    //    {
    //       // _environmentConfiguration = environmentConfiguration;
    //        _rabbitMqConnectionFactory = rabbitMqConnectionFactory;
    //        _rabbitMqConfiguration = rabbitMqConfiguration;
    //        _eventJsonSerializer = eventJsonSerializer;
    //        _dispatchToEventSubscribers = dispatchToEventSubscribers;
    //    }

    //    public void Dispose()
    //    {
    //    }

    //    public async Task StartAsync(CancellationToken cancellationToken)
    //    {
    //        var connection =
    //            await _rabbitMqConnectionFactory.CreateConnectionAsync(_rabbitMqConfiguration.Uri, cancellationToken);
    //        await connection.WithModelAsync(model => {
    //            model.ExchangeDeclare(_rabbitMqConfiguration.Exchange, ExchangeType.Fanout);
    //            model.QueueDeclare("eventflow", false, false, true, null);
    //            model.QueueBind("eventflow", _rabbitMqConfiguration.Exchange, "");

    //            var consume = new EventingBasicConsumer(model);
    //            consume.Received += (obj, @event) => {
    //                var msg = CreateRabbitMqMessage(@event);
    //                var domainEvent = _eventJsonSerializer.Deserialize(msg.Message, new Metadata(msg.Headers));

    //                _dispatchToEventSubscribers.DispatchToAsynchronousSubscribersAsync(domainEvent, cancellationToken);
    //            };


    //            model.BasicConsume("eventflow", false, consume);
    //            return Task.CompletedTask;
    //        }, cancellationToken);
    //    }

    //    public Task StopAsync(CancellationToken cancellationToken)
    //    {
    //        return Task.CompletedTask;
    //    }

    //    private static RabbitMqMessage CreateRabbitMqMessage(BasicDeliverEventArgs basicDeliverEventArgs)
    //    {
    //        var headers = basicDeliverEventArgs.BasicProperties.Headers.ToDictionary(kv => kv.Key,
    //            kv => Encoding.UTF8.GetString((byte[])kv.Value));
    //        var message = Encoding.UTF8.GetString(basicDeliverEventArgs.Body);

    //        return new RabbitMqMessage(
    //            message,
    //            headers,
    //            new Exchange(basicDeliverEventArgs.Exchange),
    //            new RoutingKey(basicDeliverEventArgs.RoutingKey),
    //            new MessageId(basicDeliverEventArgs.BasicProperties.MessageId));
    //    }
    //}

    //public interface IRabbitMqConsumerPersistanceService
    //{

    //}

    //public class RabbitMqConsumePersistanceService : IHostedService, IRabbitMqConsumerPersistanceService, ISubscribeAsynchronousTo<ExampleAggregate, WizloId, ExampleEvent>
    //{

    //    public Task StartAsync(CancellationToken cancellationToken)
    //    {
    //        return Task.CompletedTask;
    //    }

    //    public Task StopAsync(CancellationToken cancellationToken)
    //    {
    //        return Task.CompletedTask;
    //    }

    //    public Task HandleAsync(IDomainEvent<ExampleAggregate, WizloId, ExampleEvent> domainEvent, CancellationToken cancellationToken)
    //    {

    //        Console.WriteLine($"Example Updated for {domainEvent.AggregateIdentity} with MagicNumber => {domainEvent.AggregateEvent.MagicNumber}");

    //        return Task.CompletedTask;
    //    }

    //}

    class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
               .ConfigureAppConfiguration((host, config) =>
               {
                   config.SetBasePath(Directory.GetCurrentDirectory());
                   //config.AddJsonFile("appsettings.json", true, true);
                   //config.AddJsonFile($"appsettings.{host.HostingEnvironment.EnvironmentName}.json", true, true);
                   //config.AddEnvironmentVariables();
               })
               .ConfigureServices(
                   (hostcontext, services) =>
                   {
                       //var envconfig = EnvironmentConfiguration.Bind(hostcontext.Configuration);
                       //services.AddSingleton(envconfig);

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
