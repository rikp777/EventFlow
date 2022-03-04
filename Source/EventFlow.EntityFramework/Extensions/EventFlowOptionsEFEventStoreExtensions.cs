using EventFlow.EntityFramework.EventStores;
using EventFlow.EventStores;
using EventFlow.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EventFlow.EntityFramework.Extensions;

public static class EventFlowOptionsEFEventStoreExtensions
{
    public static IEventFlowOptions UseEntityFrameworkEventStore(this IEventFlowOptions eventFlowOptions)
    {
        return eventFlowOptions
            .RegisterServices(
                f => f
                    .TryAddTransient<IEventPersistence, EntityFrameworkEventPersistence<DbContext>>()
                );
    }
}