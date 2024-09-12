using ELearning_Platform.Domain.Enitities;

namespace ELearning_Platform.Domain.Repository
{
    public interface IUserRepository
    {
        Task<UserInformations> GetUserInformationsAsync(CancellationToken token);

        Task<List<UserInformations>> GetAllUsersAsync(CancellationToken token);
    }
}
