using ELearning_Platform.API.Overrides;
using ELearning_Platform.API.Hubs;
using ELearning_Platform.API.Middleware;
using ELearning_Platform.Application.Extensions;
using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Infrastructure.Authentications;
using ELearning_Platform.Infrastructure.Database;
using ELearning_Platform.Infrastructure.Services;
using ELearning_Platform.Infrastructure.StorageAccount;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using ELearning_Platform.Application.AuthPolicy;

namespace ELearning_Platform.API
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddScoped<ErrorHandlingMiddleware>();
            builder.Services.AddAuthentication(BearerTokenDefaults.AuthenticationScheme);
            builder.Services.AddAuthorizationPolicy();
            builder.Services.AddIdentityApiEndpoints<Account>()
                .AddRoles<Roles>()
                .AddEntityFrameworkStores<PlatformDb>()
                .AddSignInManager()
                .AddRoleManager<RoleManager<Roles>>()
                .AddDefaultTokenProviders();

            builder.Services.AddApplication(); //MediatR and Validations 
            builder.Services.AddCors(pr => pr.AddPolicy("corsPolicy", options =>
            {
                options.WithOrigins("http://localhost:5173")
                     .AllowCredentials()
                     .AllowAnyMethod()
                     .AllowAnyHeader()
                     .AllowAnyMethod();
            }));
            builder.Services.AddSignalR();
            builder.Services.AddInfrastructure(builder.Configuration, builder.Environment.IsDevelopment()); //for database and repository registration 
            builder.Services.AddScoped<SeederDb>();
            builder.Services.AddSingleton<BlobStorageTable>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Description = "Pleas pass your JWT TOKEN KEY",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });


                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
        {
            new OpenApiSecurityScheme()
            {
                Name = "Bearer",
                In = ParameterLocation.Header,
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
                });

                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });


            var app = builder.Build();
            var scope = app.Services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<SeederDb>();
            await seeder.GenerateRolesAsync();

            if (app.Environment.IsEnvironment("Test"))
            {
                var testScope = app.Services.CreateScope();
                var testSeeder = testScope.ServiceProvider.GetRequiredService<TestSeederDb>();
                await testSeeder.AddTestUserAsync();
            }
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
            app.UseCors("corsPolicy");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization(); //Add to Avoid problem with Identity  
            app.MapControllers();
            app.MapHub<Notification>("/hub/notifications");
            app.MapIdentityApiFilterable<Account>(new IdentityApiEndpointRouteBuilderOptions
            {
                ExcludeRegisterPost = false,
                ExcludeLoginPost = true,
                ExcludeRefreshPost = true,
                ExcludeConfirmEmailGet = false,
                ExcludeResendConfirmationEmailPost = false,
                ExcludeForgotPasswordPost = true,
                ExcludeResetPasswordPost = true,
                ExcludeManageGroup = true,
                Exclude2faPost = false,
                ExcludegInfoGet = true,
                ExcludeInfoPost = true,
            });
            app.Run();
        }
    }
}