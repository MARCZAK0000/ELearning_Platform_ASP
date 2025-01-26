using ELearning_Platform.Domain.Database.Enitities;

namespace ELearning_Platform.Domain.Core.Repository
{
    public interface INotificationDecorator
    {
        Task SendNotificaiton((string email, string userID) currentUser, List<Notification> list, CancellationToken token);
    }
}
