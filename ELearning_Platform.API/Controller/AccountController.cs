using ELearning_Platform.Application.Services.AccountServices.Command.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ELearning_Platform.API.Controller
{
    [ApiController]
    [Route("api/account")]
    public class AccountController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("registration")]
        public async Task<IActionResult> RegisterAsync(RegisterAccountAsyncCommand register, CancellationToken token)
        {
            await _mediator.Send(request: register, cancellationToken: token);

            return Created(string.Empty, new { Email = register.AddressEmail, Message = $"Your account have been created: {register.FirstName} {register.Surname}" });
        }
    }
}
