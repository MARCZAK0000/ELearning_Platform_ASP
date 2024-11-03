using ELearning_Platform.Domain.Enitities;

namespace ELearning_Platform.Domain.Repository
{
    public interface IElearningTestRepository
    {
        Task<Test> CreateTestAsync(ELearningClass eLearningClass, CancellationToken token);

        Task<Test> FindTestByIdAsync(string testId, CancellationToken token);
    }
}
