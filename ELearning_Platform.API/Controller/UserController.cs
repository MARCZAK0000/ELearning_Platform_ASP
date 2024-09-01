using MediatR;
using Microsoft.AspNetCore.Mvc;
using ELearning_Platform.Application.Services.UserServices.Queries;
using ELearning_Platform.Application.Services.UserServices.Queries.Informations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace ELearning_Platform.API.Controller
{
    [ApiController]
    [Route("api/user")]
    public class UserController(IMediator mediator):ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpGet("informations")]
        public async Task<IActionResult> GetInfromationsAboutCurrentUser(CancellationToken token)
        {
            var result = await _mediator.Send(request: new GetUserInformationsAsyncQuery(), cancellationToken: token);
            return Ok(result);
        }
    }
}
