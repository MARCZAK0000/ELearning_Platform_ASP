using ELearning_Platform.Application.Services.AccountServices.Command.SignIn;
using ELearning_Platform.Application.Services.UserServices.Command.UpdateUserInformations;
using ELearning_Platform.Infrastructure.Authorization;
using ELearning_Platform.Infrastructure.Mapper;
using ELearning_Platform.Infrastructure.Services.AccountServices.Command.Register;
using ELearning_Platform.Infrastructure.Services.SchoolServices.Command.CreateClass;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace ELearning_Platform.Infrastructure.Extensions
{
    public static class ServiceExentsion
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(PlatformDbMapper));

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(RegisterAccountAsyncCommand)));

            services.AddFluentValidationAutoValidation();

            services.AddScoped<IValidator<RegisterAccountAsyncCommand>, RegisterAccountAsyncCommandValidator>();
            services.AddScoped<IValidator<CreateClassAsyncCommand>, CreateClassAsyncCommandValidator>();
            services.AddScoped<IValidator<SignInAsyncCommand>, SignInAsyncCommandValidator>();
            services.AddScoped<IValidator<UpdateUserInformationsAsyncCommand>, UpdateUserInformationsAsyncValidator>();
            services.AddScoped<IUserContext,  UserContext>();
        }
    }
}
