using Elasticsearch.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nest;
using Pl.Core;
using Pl.Core.Entities;
using Pl.Logging.Interfaces;
using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Pl.Logging
{

    public class ElasticLogger : ILogger
    {
        private readonly ElasticLogOptions _elasticLogOptions;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IBackgroundLogTaskQueue _logtaskQueue;
        private readonly string _name;

        public ElasticLogger(string name, ElasticLogOptions elasticLogOptions, IBackgroundLogTaskQueue logTaskQueue, IHttpContextAccessor contextAccessor)
        {
            _elasticLogOptions = elasticLogOptions;
            _contextAccessor = contextAccessor;
            _name = name;
            _logtaskQueue = logTaskQueue;
        }

        internal IExternalScopeProvider ScopeProvider { get; set; }

        public IDisposable BeginScope<TState>(TState state) => ScopeProvider?.Push(state) ?? null;

        public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel)
        {
            return !_elasticLogOptions.FilterLogLevels.Contains(logLevel);
        }

        public void Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
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
                Uri[] listNodes = _elasticLogOptions.Nodes.Split(";").Select(q => new Uri(q)).ToArray();
                StaticConnectionPool pool = new StaticConnectionPool(listNodes);
                ConnectionSettings settings = new ConnectionSettings(pool);
                settings.DefaultIndex($"log-{DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)}");
                await (new ElasticClient(settings)).IndexDocumentAsync(createEntity);
            });

            string CreateFullMessage(Microsoft.Extensions.Logging.LogLevel logLevel, EventId eventId, string message, Exception exception)
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