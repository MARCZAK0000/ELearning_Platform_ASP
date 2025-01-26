
using ELearning_Platform.Domain.Core.Repository;
using ELearning_Platform.Domain.Setttings.Settings;
using ELearning_Platform.Infrastructure.Email.EmailSender.Class;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ELearning_Platform.Infrastructure.Email.Service
{
    public static class ServiceExtensions
    {
        public static void AddEmailSender(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<EmailSettings>()
               .BindConfiguration("EmailAuthentication")
               .ValidateDataAnnotations()
               .ValidateOnStart();

            services.AddSingleton(sp
                => sp.GetRequiredService<IOptionsMonitor<EmailSettings>>().CurrentValue); //Singleton Implementation but cannot be changed

            services.AddOptions<ClientSettings>()
                .BindConfiguration(nameof(ClientSettings))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddSingleton
                (sp => sp.GetRequiredService<IOptionsMonitor<ClientSettings>>().CurrentValue); //Singleton Implementation but can be changed



            services.AddScoped<IEmailSenderHelper, EmailSenderHelper>();
            services.AddSingleton<IEmailSender, EmailSender.Class.EmailSender>();
        }
    }
}
