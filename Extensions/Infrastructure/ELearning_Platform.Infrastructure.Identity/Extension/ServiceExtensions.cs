
using ELearning_Platform.Infrastructure.Identity.Authentications;
using ELearning_Platform.Infrastructure.Identity.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ELearning_Platform.Infrastructure.Identity.Extension
{
    public static class ServiceExtensions
    {
        public static void AddIdentityOptions(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {

            if (isDevelopment)
            {
                services.AddDevelopmentCorsPolicy();
            }
            else
            {
                
            }

            var authSettings = new AuthenticationSettings();
            configuration.GetSection("AuthSetting").GetSection("AuthSetting").Bind(authSettings);
            services.AddSingleton(authSettings);

            services.AddAuthorizationPolicy();
            services.AddJWTTokenAuthentication(authSettings, options =>
            {
                options.IsHttpOnly = true;
            });
        }
    }
}
