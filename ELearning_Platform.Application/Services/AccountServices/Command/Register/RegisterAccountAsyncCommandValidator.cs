using FluentValidation;

namespace ELearning_Platform.Infrastructure.Services.AccountServices.Command.Register
{
    public class RegisterAccountAsyncCommandValidator : AbstractValidator<RegisterAccountAsyncCommand>
    {
        private readonly string _chars = "1234567890";
        public RegisterAccountAsyncCommandValidator()
        {
            RuleFor(pr => pr.AddressEmail)
                .NotEmpty()
                .EmailAddress().WithMessage("Has To be an Email Address")
                .MaximumLength(100).WithMessage("Email has to have less than 100 digits");

            RuleFor(pr => pr.Password)
                .MinimumLength(6).WithMessage("Password has to have atleast 6 digits")
                .MaximumLength(18).WithMessage("Password has to have less than 18 digits")
                .NotEmpty();

            RuleFor(pr => pr.ConfirmPassword)
                .Equal(pr => pr.Password).WithMessage("Passwords have to be the same");

            RuleFor(pr => pr.FirstName)
                .NotEmpty().WithMessage("First name cannont be empty")
                .MaximumLength(50).WithMessage("First name has to have less than 50 digits")
                .MinimumLength(2).WithMessage("First name has to have atleast 2 digits");

            RuleFor(pr => pr.Surname)
                .NotEmpty().WithMessage("Surname cannont be empty")
                .MaximumLength(50).WithMessage("Surname has to have less than 50 digits")
                .MinimumLength(2).WithMessage("Surname has to have atleast 2 digits");

            RuleFor(pr => pr.City)
               .NotEmpty().WithMessage("City cannont be empty")
               .MaximumLength(50).WithMessage("City has to have less than 50 digits")
               .MinimumLength(2).WithMessage("City has to have atleast 2 digits");

            RuleFor(pr => pr.Country)
               .NotEmpty().WithMessage("Country cannont be empty")
               .MaximumLength(50).WithMessage("Country has to have less than 50 digits")
               .MinimumLength(2).WithMessage("Country has to have atleast 2 digits");

            RuleFor(pr => pr.StreetName)
               .NotEmpty().WithMessage("Street Name cannont be empty")
               .MaximumLength(50).WithMessage("Street Name has to have less than 50 digits")
               .MinimumLength(5).WithMessage("Street Name has to have atleast 2 digits");

            RuleFor(pr => pr.PostalCode)
                .NotEmpty()
                .Length(6).WithMessage("Postal Code has to have 6 digits");

            RuleFor(pr => pr.PhoneNumber)
                .Custom((value, context) =>
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        if (!_chars.Contains(value[i]))
                        {
                            context.AddFailure("Invalid digit in Phone Number");
                        }
                    }
                });



        }
    }
}
