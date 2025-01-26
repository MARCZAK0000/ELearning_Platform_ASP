using Microsoft.AspNetCore.SignalR;
namespace ELearning_Platform.Infrastructure.Notifications.Hubs
{
    public class StronglyTypedNotificationHub : Hub<INotificationClient>
    {
        public async Task SendNotification(List<string> users, string userID)
        {

        }
    }
}
