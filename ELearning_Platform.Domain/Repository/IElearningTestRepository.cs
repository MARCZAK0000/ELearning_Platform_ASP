using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Models.ELearningTestModel;

namespace ELearning_Platform.Domain.Repository
{
    public interface IElearningTestRepository
    {
        Task<Test> CreateTestAsync(string teacherID, CreateTestModel createTestModel, CancellationToken token);

        Task<Test> FindTestByIdAsync(string testId, CancellationToken token);
    }
}
