using Azure.Storage.Blobs;
using ELearning_Platform.Infrastructure.Cloud.StorageAccount;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
namespace ELearning_Platform.Infrastructure.Cloud.Services
{
    public static class ServiceExtensions
    {
        public static void AddAzureCloud(this IServiceCollection services, IConfiguration configuration, bool IsDevelopment)
        {
            if (IsDevelopment)
            {
                services.AddSingleton(_ => new BlobServiceClient(connectionString: configuration.GetConnectionString("BlobStorageConnectionString"),
                    new BlobClientOptions(version: BlobClientOptions.ServiceVersion.V2019_02_02)));

            }
            else
            {
                //TODO
            }




            //binding
            services.AddOptions<BlobStorageTablesNames>()
                .ValidateDataAnnotations()
                .BindConfiguration(nameof(BlobStorageTablesNames))
                .ValidateOnStart();
                

            services.AddSingleton(sp
                => sp.GetRequiredService<IOptions<BlobStorageTablesNames>>().Value);

        }
    }
}
