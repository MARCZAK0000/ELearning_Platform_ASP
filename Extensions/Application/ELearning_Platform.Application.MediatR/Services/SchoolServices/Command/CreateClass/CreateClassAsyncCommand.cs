using ELearning_Platform.Domain.Models.Models.SchoolModel;
using ELearning_Platform.Domain.Models.Response.ClassResponse;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ELearning_Platform.Application.MediatR.Services.SchoolServices.Command.CreateClass
{
    public class CreateClassAsyncCommand : CreateClassDto, IRequest<Results<Ok<CreateClassResponse>, ValidationProblem, ForbidHttpResult>>
    {

    }
}
