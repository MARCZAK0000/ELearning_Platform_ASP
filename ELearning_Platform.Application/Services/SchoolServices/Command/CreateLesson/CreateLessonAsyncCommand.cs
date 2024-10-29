using ELearning_Platform.Domain.Models.SchoolModel;
using MediatR;

namespace ELearning_Platform.Application.Services.SchoolServices.Command.AddSubject.CreateLesson
{
    public class CreateLessonAsyncCommand : CreateLessonDto, IRequest<bool>
    {
    }
}
