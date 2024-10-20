using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Models.Notification;

namespace ELearning_Platform.Domain.Repository
{
    public interface INotificationGateway
    {
        public Task HandleNotifications(List<string> users, CancellationToken token);
    }
}
