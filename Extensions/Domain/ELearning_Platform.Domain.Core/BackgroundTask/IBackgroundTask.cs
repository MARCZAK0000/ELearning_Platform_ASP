namespace ELearning_Platform.Domain.Core.BackgroundTask
{
    public interface IBackgroundTask
    {
        Task ExecuteAsync(object parametrs, CancellationToken token);
    }
}
