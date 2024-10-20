using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Models.Notification;

namespace ELearning_Platform.Domain.Repository
{
    public interface INotificationDecorator
    {
        Task SendNotificaiton(List<string> list, CancellationToken token);
    }
}
