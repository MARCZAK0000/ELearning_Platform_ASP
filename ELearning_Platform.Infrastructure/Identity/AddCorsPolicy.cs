using Microsoft.Extensions.DependencyInjection;

namespace ELearning_Platform.Infrastructure.Identity
{
    public static class AddCorsPolicy
    {
        public static void AddDevelopmentCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(pr => pr.AddPolicy("corsPolicy", options =>
            {
                options.WithOrigins("http://localhost:5173", "http://192.168.0.178:5173")
                     .AllowCredentials()
                     .AllowAnyMethod()
                     .AllowAnyHeader();
            }));
        }
    }
}
