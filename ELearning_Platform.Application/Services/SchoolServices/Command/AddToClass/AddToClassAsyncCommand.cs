using ELearning_Platform.Domain.Models.SchoolModel;
using ELearning_Platform.Domain.Response.ClassResponse;
using MediatR;

namespace ELearning_Platform.Application.Services.SchoolServices.Command.AddToClass
{
    public class AddToClassAsyncCommand: AddStudentToClassDto, IRequest<AddStudentToClassResponse>
    {
    }
}
