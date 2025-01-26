using ELearning_Platform.Application.Authorization.Authorization;
using ELearning_Platform.Application.MediatR.Services.AccountServices.Command.Register;
using ELearning_Platform.Application.MediatR.Services.AccountServices.Command.SignIn;
using ELearning_Platform.Application.MediatR.Services.NotificationServices.Command.CreateNotification;
using ELearning_Platform.Application.MediatR.Services.SchoolServices.Command.AddSubject;
using ELearning_Platform.Application.MediatR.Services.SchoolServices.Command.AddToClass;
using ELearning_Platform.Application.MediatR.Services.SchoolServices.Command.CreateClass;
using ELearning_Platform.Application.MediatR.Services.SchoolServices.Command.CreateTest;
using ELearning_Platform.Application.MediatR.Services.UserServices.Command.UpdateUserInformations;
using ELearning_Platform.Application.MediatR.Validation;
using ELearning_Platform.Domain.Models.Models.Pagination;
using ELearning_Platform.Infrastructure.Authorization;
using ELearning_Platform.Infrastructure.Mapper;
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
            services.AddScoped<IValidator<AddSubjectAsyncCommand>, AddSubjectAsyncCommandValidator>();
            services.AddScoped<IValidator<AddToClassAsyncCommand>, AddToClassAsyncCommandValidator>();
            services.AddScoped<IValidator<CreateTestAsyncCommand>, CreateTestAsyncCommandValidator>();
            services.AddScoped<IUserContext, UserContext>();
        }
    }
}
