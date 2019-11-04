using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pl.Core.Security;
using Pl.Core.Interfaces;
using System.Linq;

namespace Pl.System
{
    public static class SystemConfigure
    {
        /// <summary>
        /// add system data to service container, Ensure call <see cref="SystemDataProtection"/>.InitializeFileSystemProtecter fisrt to setup a data protecter
        /// </summary>
        /// <param name="services">Service container</param>
        /// <param name="dbConnection">connection string to db</param>
        /// <param name="contentRootPath">root file content and upload folder of web hosting</param>
        public static void AddSystemDataProvider(this IServiceCollection services, string dbConnection, string contentRootPath)
        {

            using var systemDbContext = new SystemDbContext(new DbContextOptionsBuilder<SystemDbContext>().UseSqlServer(dbConnection).Options);
            systemDbContext.Database.Migrate();

            DbSetting dbSetting = new DbSetting(dbConnection, contentRootPath);
            services.AddSingleton<ISystemSettings>(dbSetting);

            services.AddTransient<IActivityLogData, ActivityLogData>();
            services.AddTransient<ICmsMenuData, CmsMenuData>();
            services.AddTransient<ICurrencyData, CurrencyData>();
            services.AddTransient<IFileResourceData, FileResourceData>();
            services.AddTransient<IFileResourceMappingData, FileResourceMappingData>();
            services.AddTransient<ILanguageData, LanguageData>();
            services.AddTransient<ILanguageResourceData, LanguageResourceData>();
            services.AddTransient<IObjectLocalizedData, ObjectLocalizedData>();
            services.AddTransient<IScheduleTaskData, ScheduleTaskData>();
            services.AddTransient<ISystemValueData, SystemValueData>();

        }
    }
}
