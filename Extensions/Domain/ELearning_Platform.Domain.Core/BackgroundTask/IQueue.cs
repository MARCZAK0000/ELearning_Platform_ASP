namespace ELearning_Platform.Domain.Core.BackgroundTask
{
    public interface IQueue
    {
        void QueueBackgroundWorkItem(Func<CancellationToken, Task> task);

        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }
}
