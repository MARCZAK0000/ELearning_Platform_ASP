using ELearning_Platform.Application.Services.AccountServices.Command.SignIn;
using ELearning_Platform.Application.Services.NotificationServices.Command.CreateNotification;
using ELearning_Platform.Application.Services.UserServices.Command.UpdateUserInformations;
using ELearning_Platform.Application.Validation;
using ELearning_Platform.Domain.Models.Pagination;
using ELearning_Platform.Infrastructure.Authorization;
using ELearning_Platform.Infrastructure.Mapper;
using ELearning_Platform.Infrastructure.Services.AccountServices.Command.Register;
using ELearning_Platform.Infrastructure.Services.SchoolServices.Command.CreateClass;
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
            services.AddScoped<IValidator<CreateClassAsyncCommand>, CreateClassAsyncCommandValidator>();
            services.AddScoped<IValidator<SignInAsyncCommand>, SignInAsyncCommandValidator>();
            services.AddScoped<IValidator<UpdateUserInformationsAsyncCommand>, UpdateUserInformationsAsyncValidator>();
            services.AddScoped<IValidator<CreateNotificationAsyncCommand>, CreateNotificationCommandValidator>();
            services.AddScoped<IValidator<PaginationModelDto>, PaginationValidator>();
            services.AddScoped<IUserContext, UserContext>();
        }
    }
}
