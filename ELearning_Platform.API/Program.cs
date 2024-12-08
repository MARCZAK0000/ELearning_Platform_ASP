using ELearning_Platform.API.Azurite;
using ELearning_Platform.API.MainHubs;
using ELearning_Platform.API.Middleware;
using ELearning_Platform.API.QueueService;
using ELearning_Platform.API.Swagger;
using ELearning_Platform.Application.Extensions;
using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Infrastructure.Database;
using ELearning_Platform.Infrastructure.Services;
using ELearning_Platform.Infrastructure.StorageAccount;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ELearning_Platform.API
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddScoped<ErrorHandlingMiddleware>();
            builder.Services.AddApplication(); //MediatR and Validations 
            builder.Services.AddInfrastructure(builder.Configuration, builder.Environment.IsDevelopment()); //for database and repository registration 
            builder.Services.AddIdentityApiEndpoints<Account>()
                .AddRoles<Roles>()
                .AddEntityFrameworkStores<PlatformDb>()
                .AddSignInManager()
                .AddRoleManager<RoleManager<Roles>>()
                .AddDefaultTokenProviders();
            builder.Services.AddSignalR();
            builder.Services.AddScoped<SeederDb>();
            builder.Services.AddSingleton<BlobStorageTable>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddControllers();
            builder.Services.AddSwagger();
            builder.Services.AddHostedService<EmailNotificationBackgroundService>(); //Add BackGroundService 
            builder.Services.AddHostedService<ImageHandlerBackgroundService>();
            builder.Services.AddSingleton<RunAzurite>();
            builder.Services.AddOptions<AzuriteOptions>()
               .BindConfiguration(nameof(AzuriteOptions))
               .ValidateDataAnnotations()
               .ValidateOnStart();

            builder.Services.AddSingleton(sp
                => sp.GetRequiredService<IOptions<AzuriteOptions>>().Value);

            var app = builder.Build();
            var scope = app.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<SeederDb>();
            await seeder.GenerateRolesAsync();



            //create test enviromet for seeder

            if (app.Environment.IsEnvironment("Test"))
            {
                var testScope = app.Services.CreateScope();
                var testSeeder = testScope.ServiceProvider.GetRequiredService<TestSeederDb>();
                await testSeeder.AddTestUserAsync();
            }
            //Remove blob storage from test enviroment 
            else
            {
                if (app.Environment.IsDevelopment())
                {
                    var emulator = app.Services.GetRequiredService<RunAzurite>();
                    await emulator.RunEmulator();

                    app.UseSwagger();
                    app.UseSwaggerUI();
                }
                var blobStorage = app.Services.GetRequiredService<BlobStorageTable>();
                await blobStorage.CreateTable();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();
            //app.UseHttpsRedirection();
            app.UseCors("corsPolicy");
            app.UseAuthentication();
            app.UseAuthorization(); //Add to Avoid problem with Identity  
            app.MapControllers();
            app.MapHub<NotificationHub>("/hub/notifications");
            app.Run();
        }
    }
}