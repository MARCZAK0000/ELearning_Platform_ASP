namespace ELearning_Platform.Domain.Core.BackgroundTask
{
    public interface IAzureHandlerQueue
    {
        void QueueBackgroundWorkItem(Func<CancellationToken, Task> task);

        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }
}
