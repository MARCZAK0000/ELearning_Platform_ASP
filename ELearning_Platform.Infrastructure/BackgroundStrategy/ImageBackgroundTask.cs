using ELearning_Platform.Domain.BackgroundTask;
using ELearning_Platform.Domain.Models.UserAddress;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ELearning_Platform.Infrastructure.BackgroundStrategy
{
    public class ImageBackgroundTask(IAzureRepository azureRepository, 
        IHubContext<StronglyTypedNotificationHub, INotificationClient> hubContext):IBackgroundTask
    {
        private readonly IAzureRepository _azureRepository = azureRepository;

        private readonly IHubContext<StronglyTypedNotificationHub, INotificationClient> _hubContext = hubContext;

        public async Task ExecuteAsync(object parametrs, CancellationToken token)
        {
            var dto = parametrs as UpdateUserImageProfileDto;
            if(dto == null)
            {
                ArgumentException.ThrowIfNullOrEmpty(nameof(dto));
            }
            var result = await _azureRepository.UploadUserImage(dto!.Image, dto.UserID, token);
            if (result)
            {
                //await _hubContext.Clients.Client(dto.UserID).ReciveMessage(new Domain.Response.Notification.GetNotificationModelDto()
                //{
                //    Ge
                //});
            }
            
        }
    }
}
