using ELearning_Platform.Domain.Models.Models.SchoolModel;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ELearning_Platform.Application.MediatR.Services.SchoolServices.Command.AddToClass
{
    public class AddToClassAsyncCommand : AddStudentToClassDto, IRequest<Results<Ok, ValidationProblem, NotFound<ProblemDetails>, ForbidHttpResult>>
    {
    }
}
