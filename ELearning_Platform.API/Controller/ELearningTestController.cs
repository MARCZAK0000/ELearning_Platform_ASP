using Azure.Storage.Sas;
using ELearning_Platform.Application.Services.ELearningTestServices.Query.GetTest;
using ELearning_Platform.Application.Services.ELearningTestServices.Query.GetTestsBySubjectID;
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

        [Authorize(Policy = PolicyConstant.RequireStudent)]
        [HttpGet("find/test/byid")]
        public async Task<IActionResult> FindTestByID([FromQuery] GetTestByIDAsyncQuery request, CancellationToken token)
            => Ok(await _mediator.Send(request,token));

        [Authorize(Policy=PolicyConstant.RequireStudent)]
        [HttpGet("find/test/bysubject")]
        public async Task<IActionResult> FindTestsBySubjectID([FromQuery] FindTestsBySubjectIDAsyncQuery request, CancellationToken token)
            =>Ok(await _mediator.Send(request,token));
    }
} 