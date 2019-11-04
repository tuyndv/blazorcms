using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pl.Logging.Interfaces;
using System;

namespace Pl.Logging
{
    public class DbLoggerProvider : ILoggerProvider
    {
        private readonly DbLogOptions _dbLogOptions;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IBackgroundLogTaskQueue _logTaskQueue;

        public DbLoggerProvider(IOptions<DbLogOptions> options, IHttpContextAccessor contextAccessor, IBackgroundLogTaskQueue logTaskQueue)
        {
            _dbLogOptions = options.Value;
            _contextAccessor = contextAccessor;
            _logTaskQueue = logTaskQueue;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new DbLogger(categoryName, _dbLogOptions, _logTaskQueue, _contextAccessor);
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
