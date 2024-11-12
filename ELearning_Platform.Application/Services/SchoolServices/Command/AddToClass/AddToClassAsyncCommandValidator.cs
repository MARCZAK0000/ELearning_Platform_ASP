using ELearning_Platform.Infrastructure.Services.SchoolServices.Command.AddToClass;
using FluentValidation;

namespace ELearning_Platform.Application.Services.SchoolServices.Command.AddToClass
{
    public class AddToClassAsyncCommandValidator : AbstractValidator<AddToClassAsyncCommand>
    {
        public AddToClassAsyncCommandValidator()
        {
            RuleFor(pr=>pr.ClassID)
                .NotEmpty();

            RuleFor(pr=>pr.UsersToAdd)
                .NotEmpty();

        }
    }
}
