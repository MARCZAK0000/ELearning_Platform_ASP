using ELearning_Platform.Infrastructure.BackgroundStrategy;

namespace ELearning_Platform.Infrastructure.QueueService
{
    public interface IBackgroundTaskQueue
    {
        void QueueBackgroundWorkItem(Func<CancellationToken, Task> task);

        Task <Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }
}
