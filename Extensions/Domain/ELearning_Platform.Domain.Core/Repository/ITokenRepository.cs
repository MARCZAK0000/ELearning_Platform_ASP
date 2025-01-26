using ELearning_Platform.Domain.Models.Models.AccountModel;
using ELearning_Platform.Domain.Setttings.Settings;

namespace ELearning_Platform.Domain.Core.Repository
{

    public interface ITokenRepository
    {
        void SetCookiesInsideResponse(TokenModelDto tokenModel);
        void RemoveCookies();
        Task<string> GenerateTokenAsync(ClaimsInformations tokenInformations, IList<string> roles);
        string GetRefreshTokenFromContext();
    }
}
