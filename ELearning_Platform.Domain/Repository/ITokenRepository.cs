using ELearning_Platform.Domain.Authentication;
using ELearning_Platform.Domain.Models.AccountModel;

namespace ELearning_Platform.Domain.Repository
{

    public interface ITokenRepository
    {
        void SetCookiesInsideResponse(TokenModelDto tokenModel);
        Task<string> GenerateTokenAsync(ClaimsInformations tokenInformations, IList<string> roles);
    }
}
