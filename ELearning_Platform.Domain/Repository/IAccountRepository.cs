using ELearning_Platform.Domain.Models.AccountModel;
using ELearning_Platform.Domain.Response.Account;

namespace ELearning_Platform.Domain.Repository
{
    public interface IAccountRepository
    {
        Task RegisterAccountAsync(RegisterModelDto registerModelDto, CancellationToken cancellationToken);

        Task<LoginResponse> SignInAsync(LoginModelDto loginModelDto, CancellationToken cancellationToken);

        Task<LoginResponse> RefreshTokenAsync(RefreshTokenModelDto refreshTokenModelDto, CancellationToken cancellationToken);
    }
}
