using EventFlow.Configuration;
using System;
using EventFlowExample;
using MongoDB.Driver;

namespace EventFlow.Tests
{
    public abstract class IntegrationTestBase : IDisposable
    {
        #region Variables
        public ICommandBus CommandBus { get; set; }
        public const string SNAPSHOT_CONTAINER_NAME = "snapshots";
        public Uri RabbitMqUri = new Uri(@"amqp://test:test@localhost:5672");
        public MongoClient MongoClient => new MongoClient("mongodb://localhost:27017");
        #endregion

        public void DecorateCommandBus(IServiceRegistration sr)
        {
            sr.Decorate<ICommandBus>((r, cb) => new LogCommandBus(cb));
        }

        public virtual void Dispose(bool isDisposing)
        {
            if (!isDisposing)
                return;
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
