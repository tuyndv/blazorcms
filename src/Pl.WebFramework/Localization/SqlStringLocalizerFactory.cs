using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;

namespace Pl.WebFramework.Localization
{
    public class SqlStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly SqlLocalizationOptions _sqlLocalizationOptions;
        private readonly IBackgroundTaskQueue _taskQueue;

        public SqlStringLocalizerFactory(IBackgroundTaskQueue taskQueue, IOptions<SqlLocalizationOptions> options)
        {
            _taskQueue = taskQueue;
            _sqlLocalizationOptions = options.Value;
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            var key = (baseName + location).ToLower();
            return new SqlStringLocalizer(key, _sqlLocalizationOptions, _taskQueue);
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            return Create(resourceSource?.Name ?? "ShareResource", string.Empty);
        }
    }
}
