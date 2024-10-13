
using ELearning_Platform.Domain.BackgroundTask;
using ELearning_Platform.Domain.Exceptions;

namespace ELearning_Platform.API.QueueService
{
    public class ImageHandlerBackgroundService(IImageHandlerQueue queue) : BackgroundService
    {
        private readonly IImageHandlerQueue _queue = queue;
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
