using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Models.Notification;

namespace ELearning_Platform.Domain.Repository
{
    public interface INotificationDecorator
    {
        Task SendNotificaiton((string email, string userID) currentUser, List<Notification> list, CancellationToken token);
    }
}
