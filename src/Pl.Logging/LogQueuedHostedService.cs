using Microsoft.Extensions.Hosting;
using Pl.Logging.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pl.Logging
{
    public class LogQueuedHostedService : BackgroundService
    {
        public LogQueuedHostedService(IBackgroundLogTaskQueue logTaskQueue)
        {
            LogTaskQueue = logTaskQueue;
        }

        public IBackgroundLogTaskQueue LogTaskQueue { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var logWorkItem = await LogTaskQueue.DequeueAsync(stoppingToken);
                try
                {
                    await logWorkItem(stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error occurred executing {nameof(logWorkItem)}: {ex.ToString()}");
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await base.StopAsync(stoppingToken);
        }
    }
}
