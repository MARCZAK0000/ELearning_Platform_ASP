using ELearning_Platform.Application.CustomAttributes;
using ELearning_Platform.Application.MediatR.Services.SchoolServices.Command.AddSubject;
using ELearning_Platform.Application.MediatR.Services.SchoolServices.Command.AddToClass;
using ELearning_Platform.Application.MediatR.Services.SchoolServices.Command.CreateClass;
using ELearning_Platform.Application.MediatR.Services.SchoolServices.Command.CreateLesson;
using ELearning_Platform.Application.MediatR.Services.SchoolServices.Query.GetClassByID;
using ELearning_Platform.Application.MediatR.Services.SchoolServices.Query.GetSubjectByID;
using ELearning_Platform.Application.MediatR.Services.SchoolServices.Query.LessonByID;
using ELearning_Platform.Infrastructure.Identity.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELearning_Platform.API.Controller
{

    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/school")]
    public class SchoolController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [Authorize(Policy = PolicyConstant.RequireModerator)]
        [Transaction]
        [HttpPost("class/create")]
        public async Task<IActionResult> CreateClass(CreateClassAsyncCommand command, CancellationToken cancellationToken)
            => Ok(value: await _mediator.Send(request: command, cancellationToken: cancellationToken));

        [Authorize(Policy = PolicyConstant.RequireTeacher)]
        [Transaction]
        [HttpPost("class/students/add")]
        public async Task<IActionResult> AddStudentToClass(AddToClassAsyncCommand command, CancellationToken token)
            => Ok(await _mediator.Send(request: command, cancellationToken: token));

        [Authorize(Policy = PolicyConstant.RequireTeacher)]
        [Transaction]
        [HttpPost("class/subject")]
        public async Task<IActionResult> CreateSubject([FromBody] AddSubjectAsyncCommand command, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(request: command, cancellationToken: cancellationToken));

        [Authorize(Policy = PolicyConstant.RequireTeacher)]
        [Transaction]
        [HttpPost("class/subject/lesson")]
        public async Task<IActionResult> CreateLesson([FromForm] CreateLessonAsyncCommand command, [FromForm] List<IFormFile> materials, CancellationToken cancellationToken)
        {
            command.Materials = materials;
            return Ok(await _mediator.Send(request: command, cancellationToken: cancellationToken));
        }

        [Authorize(Policy = PolicyConstant.RequireStudent)]
        [HttpGet("class/subject/lesson/find/id")]
        public async Task<IActionResult> GetLessonByID([FromQuery] GetLessonByIDAsyncQuery request, CancellationToken token)
            => Ok(await _mediator.Send(request, cancellationToken: token));

        [Authorize(Policy = PolicyConstant.RequireStudent)]
        [HttpGet("class/subject/find/id")]
        public async Task<IActionResult> GetSubjectByID([FromQuery] GetSubjectByIDAsyncQuery request, CancellationToken token)
            => Ok(await _mediator.Send(request, token));

        [Authorize(Policy = PolicyConstant.RequireStudent)]
        [HttpGet("class/find/id")]
        public async Task<IActionResult> GetClassByID([FromQuery] GetClassByIDAsyncQuery request, CancellationToken token)
            => Ok(await _mediator.Send(request, token));

        [Authorize(Policy = PolicyConstant.RequireStudent)]
        [HttpGet("class/subject/lesson/find/subject")]
        public async Task<IActionResult> GetLessonBySubjectID([FromQuery] string subjectID, CancellationToken token)
            => throw new NotImplementedException();
    }
}
