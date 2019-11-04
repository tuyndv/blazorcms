using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pl.WebFramework
{
    public class QueuedHostedService : BackgroundService
    {
        private readonly ILogger<QueuedHostedService> _logger;

        public QueuedHostedService(IBackgroundTaskQueue taskQueue, ILogger<QueuedHostedService> logger)
        {
            TaskQueue = taskQueue;
            _logger = logger;
        }

        public IBackgroundTaskQueue TaskQueue { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem = await TaskQueue.DequeueAsync(stoppingToken);
                try
                {
                    await workItem(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Background task error occurred executing {nameof(workItem)}");
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await base.StopAsync(stoppingToken);
        }
    }
}
