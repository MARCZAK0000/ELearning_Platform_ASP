using ELearning_Platform.Application.AuthPolicy;
using ELearning_Platform.Application.Services.SchoolServices.Command.AddToClass;
using ELearning_Platform.Application.Services.SchoolServices.Command.CreateClass;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELearning_Platform.API.Controller
{
    [ApiController]
    [Route("api/school")]
    public class SchoolController(IMediator mediator):ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [Authorize(Policy = PolicyConstant.RequireModerator)]
        [HttpPost("class/create")]
        public async Task<IActionResult> CreateClass(CreateClassAsyncCommand command, CancellationToken cancellationToken)
            => Ok(value: await _mediator.Send(request: command, cancellationToken: cancellationToken));

        [Authorize(Policy =PolicyConstant.RequireTeacher)]
        [HttpPost("class/students/add")]
        public async Task<IActionResult> AddStudentToClass(AddToClassAsyncCommand command, CancellationToken token) 
            => Ok(await _mediator.Send(request: command, cancellationToken: token));
    }
}
