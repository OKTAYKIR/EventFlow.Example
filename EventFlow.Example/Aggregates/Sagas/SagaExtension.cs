using EventFlow;
using EventFlow.Extensions;
using EventFlowExample.Aggregates.Sagas.Events;

namespace EventFlowExample.Aggregates.Sagas
{
    public static class SagaExtension
    {
        public static IEventFlowOptions ConfigureSagas(this IEventFlowOptions options)
        {
            IEventFlowOptions eventFlowOptions = options.AddEvents(typeof(ExampleSagaStartedEvent))
                                                  .AddEvents(typeof(ExampleSagaCompletedEvent))
                                                  .AddSagas(typeof(ExampleSaga))
                                                  .RegisterServices(sr =>
                                                  {
                                                      sr.RegisterType(typeof(OrderSagaLocator));
                                                  });

            return eventFlowOptions;
        }
    }
}
