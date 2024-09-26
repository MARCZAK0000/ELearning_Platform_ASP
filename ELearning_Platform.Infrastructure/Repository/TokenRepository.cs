using ELearning_Platform.Domain.Authentication;
using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Models.AccountModel;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Infrastructure.Authentications;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ELearning_Platform.Infrastructure.Repository
{
    public class TokenRepository(IHttpContextAccessor httpContext, AuthenticationSettings authenticationSettings) : ITokenRepository
    {
        private readonly IHttpContextAccessor _httpContext = httpContext;
        private readonly AuthenticationSettings _authenticationSettings = authenticationSettings;

        public Task<string> GenerateTokenAsync(ClaimsInformations tokenInformations, IList<string> roles)
        {
            var claims = new List<Claim>()
            {
                new(type: ClaimTypes.NameIdentifier, value: tokenInformations.AccountId),
                new(type: ClaimTypes.Email, value: tokenInformations.Email),
                new(type: ClaimTypes.HomePhone, value: tokenInformations.PhoneNumber),
                new(type: ClaimTypes.Surname, value: tokenInformations.Surname),
                new(type: ClaimTypes.Country, value: tokenInformations.Country),
                new(type: "City", value: tokenInformations.City),
            };

            foreach (var item in roles)
            {
                claims.Add(
                    new Claim(type: ClaimTypes.Role, value: item));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.Key));
            var credentails = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expireDays = DateTime.UtcNow.AddHours(_authenticationSettings.ExpireMinutes);

            var token = new JwtSecurityToken(
                issuer: _authenticationSettings.Issure,
                audience: _authenticationSettings.Audience,
                claims: claims,
                notBefore: null,
                expires: expireDays,
                credentails);

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public void SetCookiesInsideResponse(TokenModelDto tokenModel)
        {
            var context = _httpContext.HttpContext!;

            context.Response.Cookies.Append("accessToken", tokenModel.AccessToken,
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddMinutes(5),
                HttpOnly = true,
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            context.Response.Cookies.Append("refreshToken", tokenModel.RefreshToken,
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(7),
                HttpOnly = true,
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });
        }
    }
}
