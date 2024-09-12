using ELearning_Platform.Domain.Models.SchoolModel;
using ELearning_Platform.Domain.Response.ClassResponse;

namespace ELearning_Platform.Domain.Repository
{
    public interface ISchoolRepository
    {
        Task<CreateClassResponse> CreateClassAsync(CreateClassDto createClass, CancellationToken token);

        Task<AddStudentToClassResponse> AddStudentToClassAsync(AddStudentToClassDto addToClass, CancellationToken token);
    }
}
