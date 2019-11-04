using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.WebEncoders;
using Pl.Core.Exceptions;
using Pl.Identity;
using Pl.System;
using System.Globalization;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Pl.WebFramework.Localization
{
    public static class SqlLocalizationConfigure
    {
        /// <summary>
        /// add sql localized
        /// </summary>
        /// <param name="services">Service container</param>
        /// <param name="dbConnection">connection string to db localized</param>
        public static void AddSqlLocalizationProvider(this IServiceCollection services, string dbConnection)
        {
            services.Configure<WebEncoderOptions>(webEncoderOptions => webEncoderOptions.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All));

            using var systemDbContext = new SystemDbContext(new DbContextOptionsBuilder<SystemDbContext>().UseSqlServer(dbConnection).Options);
            SqlStringLocalizer.SetResourceLocalizations(systemDbContext.LanguageResources.ToList());
            services.Configure<SqlLocalizationOptions>(option => option.UseSettings(dbConnection, true));
            services.AddSingleton<IStringLocalizerFactory, SqlStringLocalizerFactory>();
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var systemLanguages = systemDbContext.Languages.Where(q => q.Published || q.DisplayDefault).OrderByDescending(q => q.DisplayOrder);
                var defaultLanguage = systemLanguages.FirstOrDefault(q => q.DisplayDefault);

                GuardClausesParameter.NullOrEmpty(systemLanguages, nameof(systemLanguages));
                GuardClausesParameter.Null(defaultLanguage, nameof(defaultLanguage));

                var supportedCultures = systemLanguages.Select(q => new CultureInfo(q.Culture)).ToList();
                options.DefaultRequestCulture = new RequestCulture(new CultureInfo(defaultLanguage.Culture));
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
                {
                    string cultureKey = context.Request.Query["culture"];
                    if (string.IsNullOrWhiteSpace(cultureKey))
                    {
                        cultureKey = context.Request?.Cookies[IdentityConstants.LanguageSessionCookieKey];
                    }
                    if (string.IsNullOrWhiteSpace(cultureKey))
                    {
                        cultureKey = defaultLanguage.Culture;
                    }
                    return Task.FromResult(new ProviderCultureResult(cultureKey));
                }));
            });

            //need startup website call following code
            //services.AddControllersWithViews().AddMvcLocalization().AddDataAnnotationsLocalization();
        }

        /// <summary>
        /// Use sql localization in application builder
        /// </summary>
        /// <param name="applicationBuilder">current application builder</param>
        public static void UseSqlLocalization(this IApplicationBuilder applicationBuilder)
        {
            var locOptions = applicationBuilder.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            applicationBuilder.UseRequestLocalization(locOptions.Value);
        }
    }
}
