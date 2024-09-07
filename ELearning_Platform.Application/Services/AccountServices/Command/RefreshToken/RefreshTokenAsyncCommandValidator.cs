using FluentValidation;

namespace ELearning_Platform.Application.Services.AccountServices.Command.RefreshToken
{
    public class RefreshTokenAsyncCommandValidator : AbstractValidator<RefreshTokenAsyncCommand>
    {
        public RefreshTokenAsyncCommandValidator()
        {
            RuleFor(pr=>pr.RefreshToken)
                .NotEmpty().WithMessage("Empty Refresh Token")
                .Length(15).WithMessage("Invalid Refresh Token");
        }
    }
}
