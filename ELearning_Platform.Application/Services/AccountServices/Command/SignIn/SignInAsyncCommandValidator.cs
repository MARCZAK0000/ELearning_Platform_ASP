using FluentValidation;

namespace ELearning_Platform.Application.Services.AccountServices.Command.SignIn
{
    public class SignInAsyncCommandValidator : AbstractValidator<SignInAsyncCommand>
    {
        public SignInAsyncCommandValidator()
        {
            RuleFor(pr => pr.EmailAddress)
                .NotEmpty()
                .EmailAddress().WithMessage("Has To be an Email Address")
                .MaximumLength(100).WithMessage("Email has to have less than 100 digits");

            RuleFor(pr => pr.Password)
                .MinimumLength(6).WithMessage("Password has to have atleast 6 digits")
                .MaximumLength(18).WithMessage("Password has to have less than 18 digits")
                .NotEmpty();
        }
    }
}
