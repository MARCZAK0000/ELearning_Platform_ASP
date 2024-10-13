using ELearning_Platform.API.MainHubs;
using ELearning_Platform.API.Middleware;
using ELearning_Platform.API.Swagger;
using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Infrastructure.Database;
using ELearning_Platform.Infrastructure.Extensions;
using ELearning_Platform.Infrastructure.Services;
using ELearning_Platform.Infrastructure.StorageAccount;
using Microsoft.AspNetCore.Identity;

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
                var blobStorage = app.Services.GetRequiredService<BlobStorageTable>();
                await blobStorage.CreateTable();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
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