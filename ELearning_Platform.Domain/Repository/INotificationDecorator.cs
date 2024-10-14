using ELearning_Platform.Domain.Enitities;

namespace ELearning_Platform.Domain.Repository
{
    public interface INotificationDecorator
    {
        Task SendNotificaiton(List<Notification> notification, CancellationToken token);
    }
}
