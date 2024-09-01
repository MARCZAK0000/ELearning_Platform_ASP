using ELearning_Platform.Domain.Enitities;

namespace ELearning_Platform.Domain.Repository
{
    public interface IUserRepository
    {
        public Task<UserInformations> GetUserInformationsAsync(CancellationToken token);
    }
}
