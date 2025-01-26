using ELearning_Platform.Domain.Models.Response.Notification;

namespace ELearning_Platform.Infrastructure.Notifications.Hubs
{
    public interface INotificationClient
    {
        Task ReciveNotification(GetNotificationModelDto getNotification);
    }
}
