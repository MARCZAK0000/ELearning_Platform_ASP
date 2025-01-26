using ELearning_Platform.Infrastructure.Database.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace ELearning_Platform.Infrastructure.Database.Extensions
{
    public static class ServiceExtension
    {
        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration, bool IsDevelopment)
        {
            if(IsDevelopment)
            {
                services.AddDbContext<PlatformDb>(options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("MyConnectionString"));
                    options.EnableSensitiveDataLogging();
                });
            }
            else
            {
                //todo
            }
            
        }
    }
}
