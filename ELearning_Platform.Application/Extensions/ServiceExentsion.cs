using ELearning_Platform.Application.Mapper;
using ELearning_Platform.Application.Services.AccountServices.Command.Register;
using ELearning_Platform.Application.Services.AccountServices.Command.SignIn;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace ELearning_Platform.Application.Extensions
{
    public static class ServiceExentsion
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(PlatformDbMapper));

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(RegisterAccountAsyncCommand)));

            services.AddFluentValidationAutoValidation();

            services.AddScoped<IValidator<RegisterAccountAsyncCommand>, RegisterAccountAsyncCommandValidator>();
            services.AddScoped<IValidator<SignInAsyncCommand>, SignInAsyncCommandValidator>();
        }
    }
}
