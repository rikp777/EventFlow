// The MIT License (MIT)
// 
// Copyright (c) 2015-2021 Rasmus Mikkelsen
// Copyright (c) 2015-2021 eBay Software Foundation
// https://github.com/eventflow/EventFlow
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using EventFlow.Configuration;
using EventFlow.EntityFramework.EventStores;
using EventFlow.EntityFramework.ReadStores;
using EventFlow.EntityFramework.ReadStores.Configuration;
using EventFlow.EntityFramework.ReadStores.Configuration.Includes;
using EventFlow.EntityFramework.SnapshotStores;
using EventFlow.EventStores;
using EventFlow.Extensions;
using EventFlow.ReadStores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EventFlow.EntityFramework.Extensions
{
    public static class EventFlowOptionsEFExtensions
    {
        
        public static IEventFlowOptions ConfigureEntityFramework(
            this IEventFlowOptions eventFlowOptions,
            IEntityFrameworkConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            return eventFlowOptions.RegisterServices(f =>
                {
                    f.TryAddSingleton(_ => configuration);
                }
            );
        }

        public static IEventFlowOptions UseEntityFrameworkEventStore<TDbContext>(
            this IEventFlowOptions eventFlowOptions)
            where TDbContext : DbContext
        {
            return eventFlowOptions
                .UseEventPersistence<EntityFrameworkEventPersistence<TDbContext>>();
        }

        public static IEventFlowOptions UseEntityFrameworkSnapshotStore<TDbContext>(
            this IEventFlowOptions eventFlowOptions)
            where TDbContext : DbContext
        {
            return eventFlowOptions
                .UseSnapshotPersistence<EntityFrameworkSnapshotPersistence<TDbContext>>(ServiceLifetime.Transient);
        }

        public static IEventFlowOptions AddDbContextProvider<TDbContext, TContextProvider>(
            this IEventFlowOptions eventFlowOptions,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TContextProvider : class, IDbContextProvider<TDbContext>
            where TDbContext : DbContext
        {
            return eventFlowOptions.RegisterServices(s =>
                s.AddSingleton<IDbContextProvider<TDbContext>, TContextProvider>());
        }
    }
}