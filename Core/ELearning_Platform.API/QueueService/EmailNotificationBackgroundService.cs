using ELearning_Platform.Domain.Core.BackgroundTask;
using Microsoft.Extensions.Hosting;

namespace ELearning_Platform.API.QueueService
{
    public class EmailNotificationBackgroundService(IEmailNotificationHandlerQueue backgroundTaskQueue) : BackgroundService
    {
        private readonly IEmailNotificationHandlerQueue _backgroundTaskQueue = backgroundTaskQueue;

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
