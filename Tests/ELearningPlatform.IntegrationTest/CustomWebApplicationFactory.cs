using ELearning_Platform.Infrastructure.Database.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IntegrationTest
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");

            builder.ConfigureServices(service =>
            {
                var db = service.FirstOrDefault(pr => pr.ServiceType == typeof(PlatformDb));

                if (db != null)
                {
                    service.Remove(db);
                }

                var blobStorage =
                service.AddDbContext<PlatformDb>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDatabase");
                });

                service.AddScoped<TestSeederDb>();
            });
        }

    }
}
