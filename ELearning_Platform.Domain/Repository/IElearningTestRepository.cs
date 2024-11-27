using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Models.ELearningTestModel;
using ELearning_Platform.Domain.Models.Pagination;
using ELearning_Platform.Domain.Response.Pagination;

namespace ELearning_Platform.Domain.Repository
{
    public interface IElearningTestRepository
    {
        Task<Test> CreateTestAsync(string teacherID, CreateTestModel createTestModel, CancellationToken token);

        Task<Test> FindTestByIdAsync(string testId, CancellationToken token);

        Task<Pagination<Test>> FindTestsByTeacherIDAsync(string teacherID,bool isComplited, PaginationModelDto paginationModelDto, CancellationToken token);

        Task<Pagination<Test>> FindTestsBySubjectIDAsync(string subjectID, PaginationModelDto paginationModelDto, CancellationToken token);
    }
}
