using ELearning_Platform.Domain.Models.SchoolModel;
using ELearning_Platform.Domain.Response.ClassResponse;
using MediatR;

namespace ELearning_Platform.Infrastructure.Services.SchoolServices.Command.AddToClass
{
    public class AddToClassAsyncCommand: AddStudentToClassDto, IRequest<bool>
    {
    }
}
