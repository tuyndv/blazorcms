using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pl.Core;
using Pl.Core.Entities;
using Pl.Logging.Interfaces;
using System;
using System.Text;

namespace Pl.Logging
{
    /// <summary>
    /// this logger using db log by insert to <see cref="DbLogDbContext"/>.SystemLogs  table mapping with <see cref="SystemLog"/>
    /// </summary>
    public class DbLogger : ILogger
    {
        private readonly DbLogOptions _dbLogOptions;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IBackgroundLogTaskQueue _logtaskQueue;
        private readonly string _name;

        public DbLogger(string name, DbLogOptions dbLogOptions, IBackgroundLogTaskQueue logTaskQueue, IHttpContextAccessor contextAccessor)
        {
            _dbLogOptions = dbLogOptions;
            _contextAccessor = contextAccessor;
            _name = name;
            _logtaskQueue = logTaskQueue;
        }

        internal IExternalScopeProvider ScopeProvider { get; set; }

        public IDisposable BeginScope<TState>(TState state) => ScopeProvider?.Push(state) ?? null;

        public bool IsEnabled(LogLevel logLevel)
        {
            return !_dbLogOptions.FilterLogLevels.Contains(logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            var logBuilder = new StringBuilder();
            var message = formatter(state, exception);
            if (string.IsNullOrEmpty(message) && exception != null)
            {
                message = exception.Message;
            }

            SystemLog createEntity = new SystemLog()
            {
                Message = CoreUtility.TruncateString(message, 512) ?? "Empty",
                FullMessage = CreateFullMessage(logLevel, eventId, message, exception) ?? "Empty",
                Level = logLevel
            };

            if (_contextAccessor?.HttpContext != null)
            {
                createEntity.IpAddress = _contextAccessor.HttpContext.GetCurrentIpAddress();
                createEntity.PageUrl = _contextAccessor.HttpContext.GetThisPageUrl();
                createEntity.ReferrerUrl = _contextAccessor.HttpContext.GetUrlReferrer();
                createEntity.UserAgent = _contextAccessor.HttpContext.Request?.Headers["User-Agent"];
            }

            _logtaskQueue.QueueBackgroundWorkItem(async token =>
            {
                using var localizedDbContext = new DbLogDbContext(new DbContextOptionsBuilder<DbLogDbContext>().UseSqlServer(_dbLogOptions.ConnectionString).Options);
                localizedDbContext.SystemLogs.Add(createEntity);
                await localizedDbContext.SaveChangesAsync();
            });

            string CreateFullMessage(LogLevel logLevel, EventId eventId, string message, Exception exception)
            {
                var logBuilder = new StringBuilder();

                var logLevelString = logLevel.ToString();
                logBuilder.Append($"{DateTime.Now.ToString(new System.Globalization.CultureInfo("vi-VN"))} - {logLevelString} - {_name}");

                if (null != eventId && eventId.Id > 0)
                {
                    logBuilder.Append($" [ {eventId.Id} - {eventId.Name ?? "null"} ] ");
                }

                AppendAndReplaceNewLine(logBuilder, message);

                if (exception != null)
                {
                    logBuilder.Append(' ');
                    AppendAndReplaceNewLine(logBuilder, exception.ToString());
                }

                return logBuilder.ToString();

                static void AppendAndReplaceNewLine(StringBuilder sb, string message)
                {
                    var len = sb.Length;
                    sb.Append(message);
                    sb.Replace(Environment.NewLine, " ", len, message.Length);
                }
            }
        }
    }
}
