using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Infrastructure.Database;

namespace ELearning_Platform.Infrastructure.Repository
{
    public class ELearningTestRepository
        (PlatformDb platformDb): IElearningTestRepository
    {
        private readonly PlatformDb _platformDb = platformDb;
        public Task<Test> CreateTestAsync(ELearningClass eLearningClass, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<Test> FindTestByIdAsync(string testId, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}