using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Infrastructure.Authentications;
using ELearning_Platform.Infrastructure.Database;
using ELearning_Platform.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ELearning_Platform.Infrastructure.Services
{
    public static class ServiceExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PlatformDb>(options =>
                options.UseSqlServer(configuration.GetConnectionString("MyConnectionString")));



            services.AddScoped<IAccountRepository, AccountRepository>();


            //binding
            var authSetings = new AuthenticationSettings();
            configuration.GetSection("").Bind(authSetings);

            services.AddSingleton(authSetings);


        }
    }
}
