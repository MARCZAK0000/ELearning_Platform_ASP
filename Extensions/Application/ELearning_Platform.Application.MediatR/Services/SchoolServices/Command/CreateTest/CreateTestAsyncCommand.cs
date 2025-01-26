using ELearning_Platform.Domain.Models.Models.ELearningTestModel;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ELearning_Platform.Application.MediatR.Services.SchoolServices.Command.CreateTest
{
    public class CreateTestAsyncCommand : CreateTestModel, IRequest<Results<Ok, BadRequest, ForbidHttpResult>>
    {

    }
}
