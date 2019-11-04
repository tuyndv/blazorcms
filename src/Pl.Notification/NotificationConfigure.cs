using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pl.Core.Interfaces;
using Pl.Notification;

namespace Pl.System
{
    public static class NotificationConfigure
    {

        /// <summary>
        /// add system notification data to service container
        /// </summary>
        /// <param name="services">Service container</param>
        /// <param name="dbConnection">connection string to notification db</param>
        public static void AddNotificationDataProvider(this IServiceCollection services, string dbConnection)
        {

            using var localizedDbContext = new NotificationDbContext(new DbContextOptionsBuilder<NotificationDbContext>().UseSqlServer(dbConnection).Options);
            localizedDbContext.Database.Migrate();

            services.AddTransient<IEmailAccountData, EmailAccountData>();
            services.AddTransient<IMessageTemplateData, MessageTemplateData>();
            services.AddTransient<IQueuedEmailData, QueuedEmailData>();

        }
    }
}
