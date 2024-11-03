using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Models.SchoolModel;
using ELearning_Platform.Domain.Response.ClassResponse;

namespace ELearning_Platform.Domain.Repository
{
    public interface ISchoolRepository
    {
        Task<CreateClassResponse> CreateClassAsync(CreateClassDto createClass, CancellationToken token);

        Task<AddStudentToClassResponse> AddStudentToClassAsync(AddStudentToClassDto addToClass, CancellationToken token);

        Task<bool> CreateSubjectAsync(string teacherID, CreateSubjectDto createSubjectDto, CancellationToken token);

        Task<Lesson> CreateLessonAsync(string userID, Subject subject, CreateLessonDto createLessonDto, CancellationToken token);

        Task<Subject> FindSubjectByTeacherIDAsync(string TeacherID, CancellationToken token);

        Task<ELearningClass> FindClassByIdAsync(string id, CancellationToken token);
    }
}
