using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pl.Logging.Interfaces;
using System;

namespace Pl.Logging
{
    public class ElaticLoggerProvider : ILoggerProvider
    {
        private readonly ElasticLogOptions _elasticLogOptions;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IBackgroundLogTaskQueue _logTaskQueue;

        public ElaticLoggerProvider(IOptions<ElasticLogOptions> options, IHttpContextAccessor contextAccessor, IBackgroundLogTaskQueue logTaskQueue)
        {
            _elasticLogOptions = options.Value;
            _contextAccessor = contextAccessor;
            _logTaskQueue = logTaskQueue;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new ElasticLogger(categoryName, _elasticLogOptions, _logTaskQueue, _contextAccessor);
        }

        protected virtual void Dispose(bool disposing)
        {
            //do not anythink
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
