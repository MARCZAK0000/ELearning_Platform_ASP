using ELearning_Platform.Domain.Models.SchoolModel;
using ELearning_Platform.Domain.Response.ClassResponse;
using MediatR;

namespace ELearning_Platform.Infrastructure.Services.SchoolServices.Command.CreateClass
{
    public class CreateClassAsyncCommand : CreateClassDto, IRequest<CreateClassResponse>
    {

    }
}
