using ELearning_Platform.Infrastructure.Mapper;
using FluentValidation;
using System.Security.Cryptography.X509Certificates;

namespace ELearning_Platform.Application.Services.NotificationServices.Command.CreateNotification
{
    public class CreateNotificationCommandValidator : AbstractValidator<CreateNotificationAsyncCommand>
    {
        public CreateNotificationCommandValidator()
        {
            RuleFor(pr => pr.Title)
                .NotEmpty().WithMessage("Is empty")
                .MaximumLength(50).WithMessage("More than 50 digits");

            RuleFor(pr => pr.Describtion)
                .NotEmpty()
                .MaximumLength(250).WithMessage("More than 250 digits");
        }
    }
}
