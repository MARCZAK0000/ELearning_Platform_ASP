using ELearning_Platform.Domain.Models.AccountModel;
using ELearning_Platform.Domain.Response.AccountResponse;

namespace ELearning_Platform.Domain.Repository
{
    public interface IAccountRepository
    {
        Task RegisterAccountAsync(RegisterModelDto registerModelDto, CancellationToken cancellationToken);

        Task<LoginResponse> SignInAsync(LoginModelDto loginModelDto, CancellationToken cancellationToken);

        Task<LoginResponse> RefreshTokenAsync(string userID, string refreshToken, CancellationToken cancellationToken);
    }
}
