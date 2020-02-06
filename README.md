# EventFlow.Example
CQRS/Event-sourcing examples using EventFlow following CQRS-ES architecture. It is configured with RabbitMQ, MongoDB(Snapshot store), PostgreSQL(Read store), EventStore(GES). It's targeted to ASP.NET Core 2.2 and include docker compose file.

## Configuration
```c#
var resolver = EventFlowOptions.New
                               .UseAutofacContainerBuilder(new ContainerBuilder())
                               .Configure(c => c.ThrowSubscriberExceptions = true)
                               .AddEvents(typeof(ExampleEvent))
                               .AddEvents(typeof(ResetEvent))
                               .AddCommands(typeof(ExampleCommand))
                               .AddCommands(typeof(ResetCommand))
                               .AddCommandHandlers(typeof(ExampleCommandHandler))
                               .AddCommandHandlers(typeof(ResetCommandHandler))
                               .ConfigureEventStore()
                               .ConfigureMongoDb(client, SNAPSHOT_CONTAINER_NAME)
                               .AddSnapshots(typeof(ExampleSnaphost))
                               .UseMongoDbSnapshotStore()
                               .RegisterServices(sr => sr.Register(i => SnapshotEveryFewVersionsStrategy.Default))
                               .RegisterServices(DecorateCommandBus)
                               .PublishToRabbitMq(RabbitMqConfiguration.With(new Uri(@"amqp://test:test@localhost:5672"), true, 4, "eventflow"))
                               .UseInMemoryReadStoreFor<Aggregates.ReadModels.ExampleReadModel>()
                               .AddJobs(typeof(ExampleJob))
                               .CreateResolver());
```
