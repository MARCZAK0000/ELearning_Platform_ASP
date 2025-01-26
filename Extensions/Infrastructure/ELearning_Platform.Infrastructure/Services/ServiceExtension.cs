using ELearning_Platform.Domain.Core.BackgroundTask;
using ELearning_Platform.Domain.Core.Repository;
using ELearning_Platform.Domain.Models.CalculateGrade;
using ELearning_Platform.Domain.Setttings.Settings;
using ELearning_Platform.Infrastructure.BackgroundStrategy;
using ELearning_Platform.Infrastructure.Cloud.Services;
using ELearning_Platform.Infrastructure.Database.Extensions;
using ELearning_Platform.Infrastructure.Email.Service;
using ELearning_Platform.Infrastructure.GradeFactory.CalculateGrade;
using ELearning_Platform.Infrastructure.Identity.Authentications;
using ELearning_Platform.Infrastructure.Identity.Extension;
using ELearning_Platform.Infrastructure.Notifications.Extension;
using ELearning_Platform.Infrastructure.Notifications.Hubs;
using ELearning_Platform.Infrastructure.QueueService;
using ELearning_Platform.Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
namespace ELearning_Platform.Infrastructure.Services
{
    public static class ServiceExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {

            services.AddDatabase(configuration, isDevelopment);
            services.AddIdentityOptions(configuration, isDevelopment);
            services.AddNotifications();
            services.AddAzureCloud(configuration, isDevelopment);
            services.AddEmailSender(configuration);






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
            services.AddScoped<ILessonMaterialsRepository, LessonMaterialsRepository>();
            services.AddScoped<IElearningTestRepository, ELearningTestRepository>();
            //Background Task
            services.AddSingleton<IEmailNotificationHandlerQueue, EmailNotificationHandlerQueue>(); //Add Email Background TaskQueue
            services.AddSingleton<IAzureHandlerQueue, AzureHandlerQueue>(); //Add Image Background Queu
            services.AddTransient<EmailBackgroundTask>();
            services.AddTransient<ImageBackgroundTask>();
            services.AddTransient<UploadFilesToAzureBackgroundTask>();
            services.AddTransient<BackgroundTask>(); //Strategy 
            services.AddTransient<Func<BackgroundEnum, IBackgroundTask>>(serviceProvider => key => //Strategy interface registration
            {
                switch (key)
                {
                    case BackgroundEnum.Email:
                        return serviceProvider.GetRequiredService<EmailBackgroundTask>();
                    case BackgroundEnum.Image:
                        return serviceProvider.GetRequiredService<ImageBackgroundTask>();
                    case BackgroundEnum.File:
                        return serviceProvider.GetRequiredService<UploadFilesToAzureBackgroundTask>();
                    default:
                        throw new ArgumentException("Invalid Service");
                }
            });


            //NotificationDecorator
            //Notification Gateway

            services.AddOptions<NotificationSettings>()
                .BindConfiguration(nameof(NotificationSettings))
                .ValidateDataAnnotations()
                .ValidateOnStart();
            services.AddSingleton
                (sp => sp.GetRequiredService<IOptionsMonitor<NotificationSettings>>().CurrentValue); //Singleton Implementation but can be changed

            services.AddScoped<INotificationGateway, NotificationGateway>();
            //Decorator
            services.AddScoped<INotificationDecorator>(serviceProvider =>
            {
                var notificationSettings = serviceProvider.GetRequiredService<NotificationSettings>();
                var emailSender = serviceProvider.GetRequiredService<IEmailSender>();
                var emailQueue = serviceProvider.GetRequiredService<IEmailNotificationHandlerQueue>();
                var emailSettings = serviceProvider.GetRequiredService<EmailSettings>();
                var hubContext = serviceProvider.GetRequiredService<IHubContext<StronglyTypedNotificationHub, INotificationClient>>();


                var pushNotifications = new PushNotification(notificationSettings, hubContext);
                var emailNotifications = new EmailNotification(notificationSettings, emailSender,
                        emailQueue, emailSettings, pushNotifications);
                var smsNotifications = new SMSNotification(notificationSettings, emailNotifications);

                return smsNotifications;
            });

            //CalculateGradeFactory
            services.AddOptions<GradeInformations>()
                .BindConfiguration(nameof(GradeInformations))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddSingleton
                (sp => sp.GetRequiredService<IOptions<GradeInformations>>().Value);

            services.AddOptions<GradePercentage>()
                .BindConfiguration(nameof(GradePercentage))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddSingleton
                (sp => sp.GetRequiredService<IOptionsMonitor<GradePercentage>>().CurrentValue);

            services.AddSingleton<ICalculateGradeFactory, CalculateGradeFactory>();
            services.AddSingleton<USACalculateGrade>(
            //    sp =>
            //{
            //    var gradeConversion = sp.GetRequiredService<GradeInformations>();
            //    var gradePercentage = sp.GetRequiredService<GradePercentage>();
            //    var usa = new USACalculateGrade()
            //}
            );
            services.AddSingleton(
                sp =>
            {
                var gradeConversion = sp.GetRequiredService<GradeInformations>();
                var gradePercentage = sp.GetRequiredService<GradePercentage>();
                var pol = new PolandCalculateGrade(gradePercentage, gradeConversion.GradeConversion);
                return pol;
            });

            services.AddSingleton<GermanCalculateGrade>();
        }
    }
}
