using ELearning_Platform.Domain.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ELearning_Platform.API.Hubs
{
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class NotificationHub(INotificaitonRepository notificaitonRepository):Hub
    {
        private readonly INotificaitonRepository _notificaitonRepository = notificaitonRepository;
        //public override Task OnConnectedAsync()
        //{
        //    //CancellationToken cancellation = CancellationToken.None;
        //    //var notifications = _notificaitonRepository.ShowNotificationsAsync(pagination: new Domain.Models.Pagination.PaginationModelDto()
        //    //{
        //    //    PageIndex = 1,
        //    //    PageSize = 10
        //    //}, token: cancellation).GetAwaiter().GetResult();
        //    //Clients.Caller.SendAsync("notifications", notifications.Items, cancellationToken: cancellation).GetAwaiter();
        //    //return base.OnConnectedAsync();
        //}
       
        public async Task HelloWorld()
        {
            await Clients.All.SendAsync("ReciveMessage", $"Hello {Context.UserIdentifier}");
        }
    }
}
