# EventFlow.Example
![Build Status](https://github.com/OKTAYKIR/EventFlow.Example/workflows/CI/badge.svg)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg)](#contributing)

CQRS/Event-sourcing examples using EventFlow following CQRS-ES architecture. It is configured with RabbitMQ, MongoDB(Snapshot store), PostgreSQL(Read store), EventStore(GES). It's targeted to .Net Core 2.2 and include [docker compose file](/build/docker-compose.yml).

## Event Sourcing/CQRS Architecture
The most common CQRS/ES architecture would look like following diagram
![OverallArchitecture](https://github.com/OKTAYKIR/EventFlow.Example/blob/master/Images/architecture_diagram.png)

The example consists of the following classes, each shown below

- Aggregates
- Command bus and commands
- Synchronous subscriber
- Event store ([GES](https://eventstore.com/))
- In-memory read model.
- Snapshots ([MongoDb](https://www.mongodb.com/))
- Sagas
- Event publising (In-memory, [RabbitMq](https://www.rabbitmq.com/))
- Metadata
- Command bus decorator, custom value object, custom execution result, ...

## Configuration
```csharp
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

## 🤝 Contributing
1. Fork it ( https://github.com/OKTAYKIR/EventFlow.Example/fork )
2. Create your feature branch (`git checkout -b my-new-feature`)
3. Commit your changes (`git commit -am 'Add some feature'`)
4. Push to the branch (`git push origin my-new-feature`)
5. Create a new Pull Request 

## Show your support
Please ⭐️ this repository if this project helped you!

## 📝 License
Apache License 2.0
