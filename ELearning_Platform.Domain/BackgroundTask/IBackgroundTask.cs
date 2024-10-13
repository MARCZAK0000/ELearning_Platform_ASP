using ELearning_Platform.Domain.Email;

namespace ELearning_Platform.Domain.BackgroundTask
{
    public interface IBackgroundTask
    {
        Task ExecuteAsync(object parametrs, CancellationToken token);
    }
}
