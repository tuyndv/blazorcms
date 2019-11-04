using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using Pl.Core.Exceptions;
using Pl.Core.Interfaces;
using System;

namespace Pl.Caching
{
    public static class CacheConfigure
    {
        /// <summary>
        /// Add memory cache service to service container
        /// </summary>
        /// <param name="services">service container</param>
        /// <param name="setupAction">memory cache options</param>
        public static void AddMemoryService(this IServiceCollection services, Action<MemoryCacheOptions> setupAction)
        {
            if (setupAction != null)
            {
                services.AddMemoryCache(setupAction);
            }
            else
            {
                services.AddMemoryCache();
            }
            services.AddTransient<IAsyncCacheService, MemoryCacheService>();
        }

        /// <summary>
        /// Add readis cache service to service container
        /// </summary>
        /// <param name="services">service container</param>
        /// <param name="setupAction">redis cache options</param>
        public static void AddRedisCacheService(this IServiceCollection services, Action<RedisCacheOptions> setupAction)
        {
            GuardClausesParameter.Null(setupAction, nameof(setupAction));
            services.AddDistributedRedisCache(setupAction);
            services.AddTransient<IAsyncCacheService, RedisCacheService>();
        }

        /// <summary>
        /// Add cache service to service container
        /// </summary>
        /// <param name="services">service container</param>
        /// <param name="setupAction">sql cache options</param>
        public static void AddCacheService(this IServiceCollection services, Action<SqlServerCacheOptions> setupAction)
        {
            GuardClausesParameter.Null(setupAction, nameof(setupAction));
            services.AddDistributedSqlServerCache(setupAction);
            services.AddTransient<IAsyncCacheService, SqlCacheService>();
        }
    }
}
