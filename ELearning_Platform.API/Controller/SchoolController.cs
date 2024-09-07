using ELearning_Platform.Application.Services.SchoolServices.Command.CreateClass;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ELearning_Platform.API.Controller
{
    [ApiController]
    [Route("api/school")]
    public class SchoolController(IMediator mediator):ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpPost("class/create")]
        public async Task<IActionResult> CreateClass(CreateClassAsyncCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request: command, cancellationToken: cancellationToken);
            return Created(string.Empty, value: (IsCreated: result.IsCreated, Name: result.Name));
        }
    }
}
