using System;
using EventFlow.EntityFramework.ReadStores;
using EventFlow.EntityFramework.ReadStores.Configuration;
using EventFlow.Extensions;
using EventFlow.ReadStores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EventFlow.EntityFramework.Extensions;

public static class EventFlowOptionsEFReadStoreExtentions
{
     /// <summary>
    /// Configures the read model. Can be used for eager loading of related data by appending .Include(..) / .ThenInclude(..) statements.
    /// </summary>
    /// <typeparam name="TReadModel">The read model's entity type</typeparam>
    /// <typeparam name="TDbContext">The database context type</typeparam>
    /// <typeparam name="TReadModelLocator">The read model locator type</typeparam>
    /// <param name="eventFlowOptions"><inheritdoc cref="IEventFlowOptions"/></param>
    /// <param name="configure">Function to configure eager loading of related data by appending .Include(..) / .ThenInclude(..) statements.</param>
    /// <remarks>Avoid navigation properties if you create read models for both, the parent entity and the child entity. Otherwise there is a risk of a ordering problem when saving aggregates and updating read modules independently (FOREIGN-KEY constraint)</remarks>
     public static IEventFlowOptions UseEntityFrameworkReadModel<TReadModel, TDbContext>
     (
        this IEventFlowOptions eventFlowOptions,
        Func<EntityFrameworkReadModelConfiguration<TReadModel>,IApplyQueryableConfiguration<TReadModel>> configure
     )
        where TDbContext : DbContext
        where TReadModel : class, IReadModel, new()
    {
        return eventFlowOptions
            .RegisterServices(f =>
            {
                f.TryAddTransient<IEntityFrameworkReadModelStore<TReadModel>, EntityFrameworkReadModelStore<TReadModel, TDbContext>>();
                f.TryAddSingleton(_ =>
                {
                    var readModelConfig = new EntityFrameworkReadModelConfiguration<TReadModel>();
                    return configure != null
                        ? configure(readModelConfig)
                        : readModelConfig;

                });
                f.TryAddTransient<IReadModelStore<TReadModel>>();
            })
            .UseReadStoreFor<IEntityFrameworkReadModelStore<TReadModel>, TReadModel>();
    }
     
    /// <summary>
    /// Configures the read model. Can be used for eager loading of related data by appending .Include(..) / .ThenInclude(..) statements.
    /// </summary>
    /// <typeparam name="TReadModel">The read model's entity type</typeparam>
    /// <typeparam name="TDbContext">The database context type</typeparam>
    /// <typeparam name="TReadModelLocator">The read model locator type</typeparam>
    /// <param name="eventFlowOptions"><inheritdoc cref="IEventFlowOptions"/></param>
    /// <param name="configure">Function to configure eager loading of related data by appending .Include(..) / .ThenInclude(..) statements.</param>
    /// <remarks>Avoid navigation properties if you create read models for both, the parent entity and the child entity. Otherwise there is a risk of a ordering problem when saving aggregates and updating read modules independently (FOREIGN-KEY constraint)</remarks>
    public static IEventFlowOptions UseEntityFrameworkReadModel<TReadModel, TDbContext, TReadModelLocator>(
        this IEventFlowOptions eventFlowOptions)
        where TDbContext : DbContext
        where TReadModel : class, IReadModel, new()
        where TReadModelLocator : IReadModelLocator
    {
        return eventFlowOptions
            .RegisterServices(f =>
            {
                f.TryAddTransient<IEntityFrameworkReadModelStore<TReadModel>,
                    EntityFrameworkReadModelStore<TReadModel, TDbContext>>();
                f.AddSingleton<IApplyQueryableConfiguration<TReadModel>>(_ => 
                    new EntityFrameworkReadModelConfiguration<TReadModel>());
                f.TryAddTransient<IReadModelStore<TReadModel>>();
            })
            .UseReadStoreFor<IEntityFrameworkReadModelStore<TReadModel>, TReadModel, TReadModelLocator>();
    }
}