using ELearning_Platform.Domain.Database.Enitities;

namespace ELearning_Platform.Domain.Core.Repository
{
    public interface INotificationGateway
    {
        public Task HandleNotifications((string email, string userID) currentUser, List<Notification> users, CancellationToken token);
    }
}
