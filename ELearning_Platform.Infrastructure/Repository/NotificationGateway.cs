using ELearning_Platform.Application.Services.NotificationServices.Query.GetNotification;
using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Models.Notification;
using ELearning_Platform.Domain.Repository;

namespace ELearning_Platform.Infrastructure.Repository
{
    /// <summary>
    /// Class used as Notification Decorator Gateway 
    /// </summary>
    /// 
    public class NotificationGateway(INotificationDecorator notificationDecorator) : INotificationGateway
    {
        private readonly INotificationDecorator _notificationDecorator = notificationDecorator;
        public async Task HandleNotifications(List<CreateNotificationDto> notification, CancellationToken token)
        {
            await _notificationDecorator.SendNotificaiton(notification, token);
        }
    }
}
