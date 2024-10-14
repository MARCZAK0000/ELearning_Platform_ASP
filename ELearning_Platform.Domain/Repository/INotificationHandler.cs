using ELearning_Platform.Domain.Enitities;

namespace ELearning_Platform.Domain.Repository
{
    public interface INotificationHandler
    {
        public Task HandleNotifications(List<Notification> notification, CancellationToken token);
    }
}
