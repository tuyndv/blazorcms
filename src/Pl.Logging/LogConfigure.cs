using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pl.Core.Interfaces;
using Pl.Logging.Interfaces;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Hosting;
using Serilog.Extensions.Logging;
using System.Text;

namespace Pl.Logging
{
    public static class LogConfigure
    {
        /// <summary>
        /// add log service provider to service container
        /// </summary>
        /// <param name="services">Service container</param>
        /// <param name="dbLogConnection">connection string to db log</param>
        public static void AddDbLog(this IServiceCollection services, string dbLogConnection)
        {
            using var localizedDbContext = new DbLogDbContext(new DbContextOptionsBuilder<DbLogDbContext>().UseSqlServer(dbLogConnection).Options);
            localizedDbContext.Database.Migrate();
            services.Configure<DbLogOptions>(option => option.UseSettings(dbLogConnection));
            services.AddSingleton<ILoggerProvider, DbLoggerProvider>();
            services.AddSingleton<IBackgroundLogTaskQueue, LogBackgroundTaskQueue>();
            services.AddHostedService<LogQueuedHostedService>();
            services.AddTransient<ISystemLogData, SystemLogData>();
        }

        /// <summary>
        /// Đăng ký setting và cấu hình service log cho  elasticlog
        /// </summary>
        /// <param name="services">Service container</param>
        /// <param name="nodes">list elatic node seperate by ,</param>
        public static void AddElasticSearchLog(this IServiceCollection services, string nodes)
        {
            services.Configure<ElasticLogOptions>(option => option.UseSettings(nodes));
            services.AddSingleton<ILoggerProvider, ElaticLoggerProvider>();
            services.AddSingleton<IBackgroundLogTaskQueue, LogBackgroundTaskQueue>();
            services.AddHostedService<LogQueuedHostedService>();
        }

        /// <summary>
        /// Register serilog
        /// </summary>
        /// <param name="services">service container</param>
        /// <param name="logPath">Path to file savelog</param>
        public static void AddSerilog(this IServiceCollection services, string logPath = "logs\\systemlog.txt")
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day, encoding: Encoding.UTF8, rollOnFileSizeLimit: true)
                .CreateLogger();
            var diagnosticContext = new DiagnosticContext(Log.Logger);
            services.AddSingleton(diagnosticContext);
            services.AddSingleton(Log.Logger);
            services.AddSingleton<ILoggerProvider, SerilogLoggerProvider>();
        }
    }
}
