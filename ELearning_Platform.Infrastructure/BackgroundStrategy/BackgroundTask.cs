using ELearning_Platform.Domain.BackgroundTask;

namespace ELearning_Platform.Infrastructure.BackgroundStrategy
{
    public class BackgroundTask(Func<BackgroundEnum, IBackgroundTask> backgroundTaskFactory)
    {
        private readonly Func<BackgroundEnum, IBackgroundTask> _backgroundTaskFactory = backgroundTaskFactory;

        public async Task ExecuteTask(BackgroundEnum type, object parameters, CancellationToken token)
        {
            var strategy = _backgroundTaskFactory(type);
            await strategy.ExecuteAsync(parameters, token);
        }
    }
}
