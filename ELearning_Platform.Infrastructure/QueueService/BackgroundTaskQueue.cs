using ELearning_Platform.Infrastructure.QueueService;
using System.Collections.Concurrent;

namespace ELearning_Platform.Infrastructure.BackgroundStrategy
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly ConcurrentQueue<Func<CancellationToken, Task>> _workitem = new();
        private readonly SemaphoreSlim _semaphore = new(0);

        public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);

            _workitem.TryDequeue(out var result);

            return result!;
        }

        public void QueueBackgroundWorkItem(Func<CancellationToken, Task> task)
        {
            ArgumentNullException.ThrowIfNull(task);

            _workitem.Enqueue(task);
            _semaphore.Release();
        }
    }
}
