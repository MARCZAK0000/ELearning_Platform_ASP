using Azure.Storage.Blobs;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Infrastructure.Authentications;
using ELearning_Platform.Infrastructure.Database;
using ELearning_Platform.Infrastructure.Repository;
using ELearning_Platform.Infrastructure.StorageAccount;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ELearning_Platform.Infrastructure.Services
{
    public static class ServiceExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {

            if (isDevelopment)
            {
                services.AddDbContext<PlatformDb>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("MyConnectionString")));

                services.AddSingleton(_ => new BlobServiceClient(connectionString: configuration.GetConnectionString("BlobStorageConnectionString"), 
                    new BlobClientOptions(version: BlobClientOptions.ServiceVersion.V2019_02_02)));
            }

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IUserRepository , UserRepository>();
            services.AddScoped<ISchoolRepository, SchoolRepository>();
            //binding
            var authSetings = new AuthenticationSettings();
            configuration.GetSection("AuthSetting").Bind(authSetings);
            services.AddSingleton(authSetings);


            var blobNames = new BlobStorageTablesNames();
            configuration.GetSection("BlobStorageTablesNames").Bind(blobNames);
            services.AddSingleton(blobNames);


        }
    }
}
