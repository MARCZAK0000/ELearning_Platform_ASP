using ELearning_Platform.Application.Services.SchoolServices.Command.AddSubject;
using ELearning_Platform.Application.Services.SchoolServices.Command.CreateLesson;
using ELearning_Platform.Application.Services.SchoolServices.Query.LessonByID;
using ELearning_Platform.Infrastructure.AuthPolicy;
using ELearning_Platform.Infrastructure.Services.SchoolServices.Command.AddToClass;
using ELearning_Platform.Infrastructure.Services.SchoolServices.Command.CreateClass;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELearning_Platform.API.Controller
{

    [ApiController]
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/school")]
    public class SchoolController(IMediator mediator):ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [Authorize(Policy = PolicyConstant.RequireModerator)]
        [HttpPost("class/create")]
        public async Task<IActionResult> CreateClass(CreateClassAsyncCommand command, CancellationToken cancellationToken)
            => Ok(value: await _mediator.Send(request: command, cancellationToken: cancellationToken));

        [Authorize(Policy = PolicyConstant.RequireTeacher)]
        [HttpPost("class/students/add")]
        public async Task<IActionResult> AddStudentToClass(AddToClassAsyncCommand command, CancellationToken token) 
            => Ok(await _mediator.Send(request: command, cancellationToken: token));

        [Authorize(Policy = PolicyConstant.RequireTeacher)]
        [HttpPost("class/subject")]
        public async Task<IActionResult> CreateSubject([FromBody] AddSubjectAsyncCommand command, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(request: command, cancellationToken: cancellationToken)); [Authorize(Policy = PolicyConstant.RequireTeacher)]

        [Authorize(Policy = PolicyConstant.RequireTeacher)]
        [HttpPost("class/subject/lesson")]
        public async Task<IActionResult> CreateLesson([FromForm] CreateLessonAsyncCommand command, [FromForm] List<IFormFile> materials, CancellationToken cancellationToken)
        {
            command.Materials = materials;
            return Ok(await _mediator.Send(request: command, cancellationToken: cancellationToken));
        }

        [Authorize(Policy =PolicyConstant.RequireStudent)]
        [HttpGet("class/subject/lesson/find/id")]
        public async Task<IActionResult> GetLessonByID([FromForm] GetLessonByIDAsyncQuery request, CancellationToken token)
            => Ok(await _mediator.Send(request, cancellationToken: token));
    }
}
