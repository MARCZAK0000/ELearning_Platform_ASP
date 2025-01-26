using ELearning_Platform.Domain.Core.BackgroundTask;
using ELearning_Platform.Domain.Exceptions.Exceptions;

namespace ELearning_Platform.API.QueueService
{
    public class ImageHandlerBackgroundService(IAzureHandlerQueue queue) : BackgroundService
    {
        private readonly IAzureHandlerQueue _queue = queue;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!stoppingToken.IsCancellationRequested)
            {
                var workitem = await _queue.DequeueAsync(stoppingToken);

                try
                {
                    await workitem(stoppingToken);
                }
                catch (Exception err)
                {
                    throw new InternalServerErrorException(err.Message);
                }
            }
        }
    }
}
