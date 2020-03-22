![CI](https://github.com/OKTAYKIR/EventFlow.Example/workflows/CI/badge.svg)
# EventFlow.Example
CQRS/Event-sourcing examples using EventFlow following CQRS-ES architecture. It is configured with RabbitMQ, MongoDB(Snapshot store), PostgreSQL(Read store), EventStore(GES). It's targeted to ASP.NET Core 2.2 and include docker compose file.

## Event Sourcing/CQRS Architecture
The most common CQRS/ES architecture would look like following diagram
![OverallArchitecture](https://github.com/OKTAYKIR/EventFlow.Example/blob/master/Images/architecture_diagram.png)

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

### Contributing
1. Fork it ( https://github.com/OKTAYKIR/EventFlow.Example/fork )
2. Create your feature branch (`git checkout -b my-new-feature`)
3. Commit your changes (`git commit -am 'Add some feature'`)
4. Push to the branch (`git push origin my-new-feature`)
5. Create a new Pull Request 
