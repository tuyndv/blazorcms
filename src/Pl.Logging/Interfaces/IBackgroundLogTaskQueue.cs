using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pl.Logging.Interfaces
{
    public interface IBackgroundLogTaskQueue
    {
        void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem);

        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }
}
