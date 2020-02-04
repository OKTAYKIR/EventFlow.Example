using System;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using EventFlow;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Extensions;
using EventFlow.Queries;
using EventFlowExample.Aggregates.CommandHandlers;
using EventFlowExample.Aggregates.Commands;
using EventFlowExample.Aggregates.Events;
using EventFlowExample.Aggregates.ReadModels;
using Xunit;

namespace EventFlowExample.Tests
{
    public class ExampleTests { 
        public static ICommandBus CommandBus { get;set;}

        [MemoryDiagnoser]
        [RPlotExporter]
        public class CommandPublish
        {
            private readonly ICommandBus commandBus;
            private readonly ExampleId exampleId;
            private readonly int magicNumber;

            public CommandPublish()
            { 
            }

            [Benchmark(Baseline = true)]
            public async Task<IExecutionResult> PublishCommand()
            {
                return await commandBus.PublishAsync(
                          new ExampleCommand(exampleId, magicNumber),
                          CancellationToken.None)
                          .ConfigureAwait(false);
            }
        }


            [Fact]
            public async Task PassingTest()
            {
                // We wire up EventFlow with all of our classes. Instead of adding events,
                // commands, etc. explicitly, we could have used the the simpler
                // AddDefaults(Assembly) instead.
                using (var resolver = EventFlowOptions.New
                                                      .AddEvents(typeof(ExampleEvent))
                                                      .AddCommands(typeof(ExampleCommand))
                                                      .AddCommandHandlers(typeof(ExampleCommandHandler))
                                                      .UseInMemoryReadStoreFor<ExampleReadModel>()
                                                      .CreateResolver())
                {
                    // Create a new identity for our aggregate root
                    var exampleId = new ExampleId(Guid.NewGuid().ToString());
                     // Resolve the command bus and use it to publish a command
                    CommandBus = resolver.Resolve<ICommandBus>();
                
                    BenchmarkRunner.Run<CommandPublish>();
                
                    //var executionResult = await PublishCommand(commandBus, exampleId, magicNumber); 

                    // Verify that we didn't trigger our domain validation
                    //executionResult.IsSuccess.Should().BeTrue();

                    // Resolve the query handler and use the built-in query for fetching
                    // read models by identity to get our read model representing the
                    // state of our aggregate root
                    var queryProcessor = resolver.Resolve<IQueryProcessor>();
                    var exampleReadModel = await queryProcessor.ProcessAsync(
                        new ReadModelByIdQuery<ExampleReadModel>(exampleId),
                        CancellationToken.None)
                        .ConfigureAwait(false);

                    // Verify that the read model has the expected magic number
                  //exampleReadModel.MagicNumber.Should().Be(42);
                }
            }
        }
        //[Test]
        //public async Task GettingStartedExample()
        //{
        //    // We wire up EventFlow with all of our classes. Instead of adding events,
        //    // commands, etc. explicitly, we could have used the the simpler
        //    // AddDefaults(Assembly) instead.
        //    using (var resolver = EventFlowOptions.New
        //        .AddEvents(typeof(ExampleEvent))
        //        .AddCommands(typeof(ExampleCommand))
        //        .AddCommandHandlers(typeof(ExampleCommandHandler))
        //        .UseInMemoryReadStoreFor<ExampleReadModel>()
        //        .CreateResolver())
        //    {
        //        // Create a new identity for our aggregate root
        //        var exampleId = ExampleId.New;

        //        // Define some important value
        //        const int magicNumber = 42;

        //        // Resolve the command bus and use it to publish a command
        //        var commandBus = resolver.Resolve<ICommandBus>();
        //        var executionResult = await commandBus.PublishAsync(
        //            new ExampleCommand(exampleId, magicNumber),
        //            CancellationToken.None)
        //            .ConfigureAwait(false);

        //        // Verify that we didn't trigger our domain validation
        //        executionResult.IsSuccess.Should().BeTrue();

        //        // Resolve the query handler and use the built-in query for fetching
        //        // read models by identity to get our read model representing the
        //        // state of our aggregate root
        //        var queryProcessor = resolver.Resolve<IQueryProcessor>();
        //        var exampleReadModel = await queryProcessor.ProcessAsync(
        //            new ReadModelByIdQuery<ExampleReadModel>(exampleId),
        //            CancellationToken.None)
        //            .ConfigureAwait(false);

        //        // Verify that the read model has the expected magic number
        //        exampleReadModel.MagicNumber.Should().Be(42);
        //    }
        //}
    }