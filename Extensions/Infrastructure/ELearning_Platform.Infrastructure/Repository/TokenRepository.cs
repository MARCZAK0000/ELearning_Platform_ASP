﻿using ELearning_Platform.Domain.Core.Repository;
using ELearning_Platform.Domain.Exceptions.Exceptions;
using ELearning_Platform.Domain.Models.Models.AccountModel;
using ELearning_Platform.Domain.Setttings.Settings;
using ELearning_Platform.Infrastructure.Identity.Authentications;
using ELearning_Platform.Infrastructure.Identity.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ELearning_Platform.Infrastructure.Repository
{
    public class TokenRepository(IHttpContextAccessor httpContext, AuthenticationSettings authenticationSettings, Action<HttpOnlyCookieOptions> cookieOptions = null) : ITokenRepository
    {
        #region Properties
        private readonly IHttpContextAccessor _httpContext = httpContext;
        private readonly AuthenticationSettings _authenticationSettings = authenticationSettings;
        #endregion

        #region Token Generation

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

        #endregion

        #region Token operations
        public string GetRefreshTokenFromContext()
        {
            var context = _httpContext.HttpContext;
            var cfg = new HttpOnlyCookieOptions();
            cookieOptions?.Invoke(cfg);
            context!.Request.Cookies.TryGetValue(cfg.RefreshTokenName, out string? refreshToken);
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new InvalidRefreshTokenException("Invalid Refresh token");
            }
            return refreshToken;
        }

        public void SetCookiesInsideResponse(TokenModelDto tokenModel)
        {
            var context = _httpContext.HttpContext!;
            var cfg = new HttpOnlyCookieOptions();
            cookieOptions?.Invoke(cfg);
            context.Response.Cookies.Append(cfg.AccessTokenName, tokenModel.AccessToken,
            new CookieOptions
            {
                Expires = cfg.AccessTokenExpireTime,
                HttpOnly = cfg.IsHttpOnly,
                IsEssential = true,
                Secure = false,
                SameSite = SameSiteMode.Lax
            });

            context.Response.Cookies.Append(cfg.RefreshTokenName, tokenModel.RefreshToken,
            new CookieOptions
            {
                Expires = cfg.RefreshTokenExpireTime,
                HttpOnly = cfg.IsHttpOnly,
                IsEssential = true,
                Secure = false,
                SameSite = SameSiteMode.Lax
            });
        }
        public void RemoveCookies()
        {
            var context = _httpContext.HttpContext!;
            var cfg = new HttpOnlyCookieOptions();
            cookieOptions?.Invoke(cfg);

            context.Response.Cookies.Delete(cfg.AccessTokenName);
            context.Response.Cookies.Delete(cfg.RefreshTokenName);
        }
        #endregion
    }
}
