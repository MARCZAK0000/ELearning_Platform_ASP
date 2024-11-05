using ELearning_Platform.Domain.Models.ELearningTestModel;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ELearning_Platform.Application.Services.SchoolServices.Command.CreateTest
{
    public class CreateTestAsyncCommand : CreateTestModel,IRequest<Results<Ok, BadRequest, ForbidHttpResult>>
    {

    }
}
