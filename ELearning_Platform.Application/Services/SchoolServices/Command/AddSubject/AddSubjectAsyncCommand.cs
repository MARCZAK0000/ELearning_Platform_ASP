using ELearning_Platform.Domain.Models.SchoolModel;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ELearning_Platform.Application.Services.SchoolServices.Command.AddSubject
{
    public class AddSubjectAsyncCommand : CreateSubjectDto, IRequest<Results<Ok<bool>, ForbidHttpResult, NotFound<ProblemDetails>>>    
    {
    }
}
