using Microsoft.Extensions.Hosting;

namespace ELearning_Platform.Infrastructure.QueueService
{
    public class CustomBackgroundSerive(IBackgroundTaskQueue backgroundTaskQueue) : BackgroundService
    {
        private readonly IBackgroundTaskQueue _backgroundTaskQueue = backgroundTaskQueue;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var workitem = await _backgroundTaskQueue.DequeueAsync(stoppingToken);
                try
                {
                    await workitem(stoppingToken);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
