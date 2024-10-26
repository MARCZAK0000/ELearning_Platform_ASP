using ELearning_Platform.Application.Services.UserServices.Command;
using ELearning_Platform.Application.Services.UserServices.Command.UpdateImage;
using ELearning_Platform.Application.Services.UserServices.Command.UpdateUserInformations;
using ELearning_Platform.Domain.Models.UserAddress;
using ELearning_Platform.Infrastructure.Services.UserServices.Queries.AllUserInformations;
using ELearning_Platform.Infrastructure.Services.UserServices.Queries.Informations;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ELearning_Platform.API.Controller
{
    [ApiController]
    [Route("api/user")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("me")]
        public async Task<IActionResult> GetInfromationsAboutCurrentUser(CancellationToken token)
        {
            var result = await _mediator.Send(request: new GetUserInformationsAsyncQuery(), cancellationToken: token);
            return Ok(result);
        }

        [HttpGet("informations/all")]
        public async Task<IActionResult> GetInfromationsAboutAllUsers([FromQuery] GetInfromationsAboutAllUsersAsyncQuery request, CancellationToken token) => 
            Ok(await _mediator.Send(request: request, cancellationToken: token));

        [HttpPut("update")]
        public async Task<IActionResult> UpdateInformtaions([FromBody] UpdateUserInformationsAsyncCommand request, CancellationToken token)=>
            Ok(await _mediator.Send(request, cancellationToken: token));

        [HttpPost("image")]
        public async Task<IActionResult> UpdateOrCreateImageProfile(IFormFile formFile, CancellationToken token)
        {

            return Ok(await _mediator.Send(new UpdateOrCreateImageProfileAsyncCommand() { Image = formFile}, cancellationToken: token));
        }
    }
}
