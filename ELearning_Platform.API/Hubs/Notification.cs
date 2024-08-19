using Microsoft.AspNetCore.SignalR;

namespace ELearning_Platform.API.Hubs
{
    public class Notification:Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
    }
}
