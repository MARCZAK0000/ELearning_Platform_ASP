using ELearning_Platform.Application.MediatR.Services.NotificationServices.Command.CreateNotification;
using ELearning_Platform.Application.MediatR.Services.NotificationServices.Command.ReadNotification;
using ELearning_Platform.Application.MediatR.Services.NotificationServices.Query.GetNotification;
using ELearning_Platform.Infrastructure.Identity.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELearning_Platform.API.Controller
{
    [ApiController]
    [Route("api/notifications")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotificationController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpPost]
        [Authorize(Policy = PolicyConstant.RequireAdmin)]
        public async Task<IActionResult> CreateNotification
            (CreateNotificationAsyncCommand request, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(request, cancellationToken));


        [HttpGet]
        [Authorize(Policy = PolicyConstant.RequireStudent)]
        public async Task<IActionResult> GetNotifications([FromQuery] GetNotificationsAsyncQuery request,
            CancellationToken cancellationToken)
            => Ok(await _mediator.Send(request, cancellationToken));

        [HttpPut]
        [Authorize(Policy = PolicyConstant.RequireStudent)]
        public async Task<IActionResult> ReadNotification([FromBody] ReadNotificationAsyncCommand request, CancellationToken token)
            => Ok(await _mediator.Send(request, token));
    }
}
