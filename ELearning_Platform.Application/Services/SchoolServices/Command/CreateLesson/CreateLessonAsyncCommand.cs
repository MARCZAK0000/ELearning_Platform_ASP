using ELearning_Platform.Domain.Models.SchoolModel;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ELearning_Platform.Application.Services.SchoolServices.Command.CreateLesson
{
    public class CreateLessonAsyncCommand : CreateLessonDto, IRequest<Results<Ok<bool>, ValidationProblem, ForbidHttpResult, NotFound<ProblemDetails>>>
    {

    }
}
