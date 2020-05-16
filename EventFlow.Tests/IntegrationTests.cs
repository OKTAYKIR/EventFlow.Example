using Autofac;
using CommandLine.Text;
using EventFlow.Aggregates;
using EventFlow.Autofac.Extensions;
using EventFlow.EventStores;
using EventFlow.Extensions;
using EventFlow.MongoDB.Extensions;
using EventFlow.Queries;
using EventFlow.RabbitMQ;
using EventFlow.RabbitMQ.Extensions;
using EventFlow.Snapshots.Strategies;
using EventFlowExample;
using EventFlowExample.Aggregates;
using EventFlowExample.Aggregates.CommandHandlers;
using EventFlowExample.Aggregates.Commands;
using EventFlowExample.Aggregates.Events;
using EventFlowExample.Aggregates.ReadModels;
using EventFlowExample.Aggregates.Snapshots;
using EventFlowExample.Jobs;
using EventFlowExample.Models;
using FluentAssertions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EventFlow.Tests
{
    public class IntegrationTests : IntegrationTestBase
    {
        [Fact]
        public async Task PassingTest()
        {
            using (var resolver = EventFlowOptions.New
                                                  .UseAutofacContainerBuilder(new ContainerBuilder())
                                                  .Configure(c => c.ThrowSubscriberExceptions = true)
                                                  .AddEvents(typeof(ExampleEvent))
                                                  .AddEvents(typeof(ResetEvent))
                                                  .AddCommands(typeof(ExampleCommand))
                                                  .AddCommands(typeof(ResetCommand))
                                                  .AddCommandHandlers(typeof(ExampleCommandHandler))
                                                  .AddCommandHandlers(typeof(ResetCommandHandler))
                                                  .ConfigureEventStore()
                                                  .ConfigureMongoDb(MongoClient, SNAPSHOT_CONTAINER_NAME)
                                                  .AddSnapshots(typeof(ExampleSnaphost))
                                                  .UseMongoDbSnapshotStore()
                                                  .UseInMemoryReadStoreFor<ExampleReadModel>()
                                                  .RegisterServices(sr => sr.Register(i => SnapshotEveryFewVersionsStrategy.Default))
                                                  .RegisterServices(DecorateCommandBus)
                                                  .PublishToRabbitMq(RabbitMqConfiguration.With(RabbitMqUri, true, 4, "eventflow"))
                                                  .Configure(c => c.IsAsynchronousSubscribersEnabled = true)
                                                  .AddJobs(typeof(ExampleJob))
                                                  .CreateResolver())
            {
                Int32 magicNumber = 2;
                CommandBus = resolver.Resolve<ICommandBus>();
                
                ExampleId exampleId = PublishCommand.GetStreamName("Tenant", "EXAMPLE");

                CommandReturnResult result = await CommandBus.PublishAsync(
                       new ExampleCommand(exampleId, magicNumber), CancellationToken.None)
                       .ConfigureAwait(false);

                IAggregateStore aggregateStore = resolver.Resolve<IAggregateStore>();
                var @aggregate = await aggregateStore.LoadAsync<ExampleAggregate, ExampleId>(exampleId, CancellationToken.None);

                //Command side
                result.IsSuccess.Should().BeTrue();
                result.AggregateRoot.Should().NotBeNull();
                result.AggregateRoot.Version.Should().Be(1);
                result.AggregateRoot.Name.Value.Should().Be("ExampleAggregate");
                result.AggregateRoot.GetIdentity().Value.Should().Be(exampleId.Value);
                @aggregate.Should().NotBeNull();
                result.AggregateRoot.Should().Equals(@aggregate);
            }
        }

        [Fact]
        public async Task ReadModelTest()
        {
            using (var resolver = EventFlowOptions.New
                                                  .UseAutofacContainerBuilder(new ContainerBuilder())
                                                  .Configure(c => c.ThrowSubscriberExceptions = true)
                                                  .AddEvents(typeof(ExampleEvent))
                                                  .AddEvents(typeof(ResetEvent))
                                                  .AddCommands(typeof(ExampleCommand))
                                                  .AddCommands(typeof(ResetCommand))
                                                  .AddCommandHandlers(typeof(ExampleCommandHandler))
                                                  .AddCommandHandlers(typeof(ResetCommandHandler))
                                                  .ConfigureEventStore()
                                                  .ConfigureMongoDb(MongoClient, SNAPSHOT_CONTAINER_NAME)
                                                  .AddSnapshots(typeof(ExampleSnaphost))
                                                  .UseMongoDbSnapshotStore()
                                                  .UseInMemoryReadStoreFor<ExampleReadModel>()
                                                  .RegisterServices(sr => sr.Register(i => SnapshotEveryFewVersionsStrategy.Default))
                                                  .RegisterServices(DecorateCommandBus)
                                                  .PublishToRabbitMq(RabbitMqConfiguration.With(RabbitMqUri, true, 4, "eventflow"))
                                                  .Configure(c => c.IsAsynchronousSubscribersEnabled = true)
                                                  .AddJobs(typeof(ExampleJob))
                                                  .CreateResolver())
            {
                Int32 magicNumber = 2;
                CommandBus = resolver.Resolve<ICommandBus>();

                ExampleId exampleId = PublishCommand.GetStreamName("Tenant", "EXAMPLE");

                CommandReturnResult result = await CommandBus.PublishAsync(
                       new ExampleCommand(exampleId, magicNumber), CancellationToken.None)
                       .ConfigureAwait(false);

                var queryProcessor = resolver.Resolve<IQueryProcessor>();
                ExampleReadModel exampleReadModel = await queryProcessor.ProcessAsync(
                     new ReadModelByIdQuery<ExampleReadModel>(exampleId),
                     CancellationToken.None)
                     .ConfigureAwait(false);

                exampleReadModel.Should().NotBeNull();
                exampleReadModel.MagicNumber.Should().ContainSingle();
                exampleReadModel.MagicNumber.First().Should().Be(2);
            }
        }
    }
}