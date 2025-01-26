using ELearning_Platform.Infrastructure.Identity.Authentications;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ELearning_Platform.Infrastructure.Identity.Identity
{
    public static class AddCustomAuthentication
    {

        public static void AddJWTTokenAuthentication(this IServiceCollection services, AuthenticationSettings authenticationSettings
            , Action<HttpOnlyCookieOptions>? cookieOptions = null)
        {
            var cfg = new HttpOnlyCookieOptions();
            cookieOptions?.Invoke(cfg);
            services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.IncludeErrorDetails = true;
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidIssuer = authenticationSettings.Issure,
                    ValidAudience = authenticationSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.Key)),

                };

                if (cfg.IsHttpOnly)
                {
                    options.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = ctx =>
                        {
                            ctx.Request.Cookies.TryGetValue("accessToken", out var accessToken);
                            if (!string.IsNullOrEmpty(accessToken))
                            {
                                ctx.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            return Task.CompletedTask;
                        }
                    };
                }
                else
                {
                    options.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = context =>
                        {
                            return Task.CompletedTask;
                        }
                    };
                }

            });
        }
    }
}
