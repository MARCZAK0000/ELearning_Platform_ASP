
namespace ELearning_Platform.Domain.BackgroundTask
{
    public interface IEmailNotificationHandlerQueue
    {
        void QueueBackgroundWorkItem(Func<CancellationToken, Task> task);

        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }
}
