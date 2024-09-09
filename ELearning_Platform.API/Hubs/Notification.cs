using Microsoft.AspNetCore.SignalR;

namespace ELearning_Platform.API.Hubs
{
    public class Notification:Hub
    {
        public async Task HelloWorld()
        {
            await Clients.All.SendAsync("ReciveMessage", "Hello");
        }
    }
}
