using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Models.SchoolModel;
using ELearning_Platform.Domain.Response.ClassResponse;

namespace ELearning_Platform.Domain.Repository
{
    public interface ISchoolRepository
    {
        Task<CreateClassResponse> CreateClassAsync(CreateClassDto createClass, CancellationToken token);

        Task<AddStudentToClassResponse> AddStudentToClassAsync(ELearningClass eClass, AddStudentToClassDto addToClass, CancellationToken token);

        Task<AddStudentToClassResponse> AddUsersToClassSubjectAsync(List<Subject> subjects, IList<string> usersToAdd, CancellationToken token);

        Task<bool> CreateSubjectAsync(string teacherID, CreateSubjectDto createSubjectDto, CancellationToken token);

        Task<Lesson> CreateLessonAsync(string userID, Subject subject, CreateLessonDto createLessonDto, CancellationToken token);

        Task<Subject> FindSubjectByTeacherIDAsync(string TeacherID, CancellationToken token);

        Task<ELearningClass?> FindClassWithStudentsByIdAsync(string id, CancellationToken token);

        Task<Subject> FindSubjectByIDAsync(string id, CancellationToken token);

        Task<Lesson?> FindLessonByIDAsync(string lessonID, string subjectID, CancellationToken token);

        Task<List<Subject>> FindSubjectByClassIDAsync(string classId,  CancellationToken token);  
        
        Task<ELearningClass?> FindClassByClassIDAsync(string classId, CancellationToken token);
    }
}
