using Azure.Storage.Blobs;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Settings;
using ELearning_Platform.Infrastructure.Authentications;
using ELearning_Platform.Infrastructure.Database;
using ELearning_Platform.Infrastructure.EmailSender.Class;
using ELearning_Platform.Infrastructure.EmailSender.Interface;
using ELearning_Platform.Infrastructure.Identity;
using ELearning_Platform.Infrastructure.BackgroundStrategy;
using ELearning_Platform.Infrastructure.Repository;
using ELearning_Platform.Infrastructure.StorageAccount;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ELearning_Platform.Domain.BackgroundTask;
using ELearning_Platform.Infrastructure.QueueService;

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

                services.AddDevelopmentCorsPolicy();
            }
            else
            {
                //TODO
            } 

            var authSettings = new AuthenticationSettings();
            configuration.GetSection("AuthSetting").Bind(authSettings);
            services.AddSingleton(authSettings);
            //Authorization and Authentication

            services.AddAuthorizationPolicy();
            services.AddJWTTokenAuthentication(authSettings, options => 
            { 
                options.IsHttpOnly = true;
            });
            //binding
            services.AddOptions<BlobStorageTablesNames>()
                .BindConfiguration(nameof(BlobStorageTablesNames))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddSingleton(sp
                =>sp.GetRequiredService<IOptions<BlobStorageTablesNames>>().Value);

            services.AddOptions<EmailSettings>()
                .BindConfiguration("EmailAuthentication")
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddSingleton(sp
                =>sp.GetRequiredService<IOptionsMonitor<EmailSettings>>().CurrentValue);

            services.AddOptions<ClientSettings>()
                .BindConfiguration(nameof(ClientSettings))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddSingleton(sp=>sp.GetRequiredService<IOptionsMonitor<ClientSettings>>().CurrentValue);

            services.AddScoped<IEmailSenderHelper, EmailSenderHelper>();
            services.AddSingleton<IEmailSender, EmailSender.Class.EmailSender>();

         
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISchoolRepository, SchoolRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>(serviceProvider =>
            {
                return 
                new TokenRepository(
                        httpContext: serviceProvider.GetRequiredService<IHttpContextAccessor>(),
                        authenticationSettings: serviceProvider.GetRequiredService<AuthenticationSettings>(),
                        cookieOptions: options =>
                        {
                            options.IsHttpOnly = true;
                            options.AccessTokenExpireTime = DateTimeOffset.Now.AddMinutes(20);
                            options.RefreshTokenExpireTime = DateTimeOffset.Now.AddHours(2);
                        }
                    );
            });
            services.AddScoped<INotificaitonRepository, NotificationReposiotry>();
            services.AddScoped<IAzureRepository, AzureRepository>();

            //Background Task
            services.AddSingleton<IEmailNotificationHandlerQueue, EmailNotificationHandlerQueue>(); //Add Email Background TaskQueue
            services.AddSingleton<IImageHandlerQueue, ImageHandlerQueue>(); //Add Image Background Queu
            services.AddTransient<EmailBackgroundTask>();
            services.AddTransient<ImageBackgroundTask>();
            services.AddTransient<BackgroundTask>(); //Strategy 
            services.AddTransient<Func<BackgroundEnum, IBackgroundTask>>(serviceProvider => key => //Strategy interface registration
            {
                switch (key)
                {
                    case BackgroundEnum.Email:
                        return serviceProvider.GetRequiredService<EmailBackgroundTask>();
                    case BackgroundEnum.Image:
                        return serviceProvider.GetRequiredService<ImageBackgroundTask>();
                    default:
                        throw new ArgumentException("Invalid Service");
                }
            });

        }
    }
}
