using ELearning_Platform.Application.Services.SchoolServices.Command.CreateTest;
using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Infrastructure.AuthPolicy;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELearning_Platform.API.Controller
{
    [ApiController]
    [Route("api/test")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ELearningTestController(IMediator mediator): ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [Authorize(Policy = PolicyConstant.RequireStudent)]
        [HttpPost("create")]
        public async Task<IActionResult> CreatTest([FromBody] CreateTestAsyncCommand request, CancellationToken token)
            => Ok(await _mediator.Send(request, token));

    }
} 