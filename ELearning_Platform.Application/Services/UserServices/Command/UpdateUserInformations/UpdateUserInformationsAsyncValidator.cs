using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ELearning_Platform.Application.Services.UserServices.Command.UpdateUserInformations
{
    public class UpdateUserInformationsAsyncValidator : AbstractValidator<UpdateUserInformationsAsyncCommand>
    {
        private readonly string _chars = "1234567890";
        public UpdateUserInformationsAsyncValidator()
        {
            RuleFor(pr => pr.FirstName)
                .NotEmpty().WithMessage("First name cannont be empty")
                .MaximumLength(50).WithMessage("First name has to have less than 50 digits")
                .MinimumLength(2).WithMessage("First name has to have atleast 2 digits");

            RuleFor(pr => pr.Surname)
                .NotEmpty().WithMessage("Surname cannont be empty")
                .MaximumLength(50).WithMessage("Surname has to have less than 50 digits")
                .MinimumLength(2).WithMessage("Surname has to have atleast 2 digits");

            RuleFor(pr => pr.SecondName)
                .Custom((value, context) =>
                {
                    if (value != null)
                    {
                        if(value.Length<=2)
                        {
                            context.AddFailure("Second name has to have atleast 2 digits");
                        }
                        if (value.Length > 50)
                        {
                            context.AddFailure("Second name has to have less than 50 digits");
                        }
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            context.AddFailure("Second name cannont be empty");
                        }
                    }
                });

            RuleFor(pr => pr.Address.City)
               .NotEmpty().WithMessage("City cannont be empty")
               .MaximumLength(50).WithMessage("City has to have less than 50 digits")
               .MinimumLength(2).WithMessage("City has to have atleast 2 digits");

            RuleFor(pr => pr.Address.Country)
               .NotEmpty().WithMessage("Country cannont be empty")
               .MaximumLength(50).WithMessage("Country has to have less than 50 digits")
               .MinimumLength(2).WithMessage("Country has to have atleast 2 digits");

            RuleFor(pr => pr.Address.StreetName)
               .NotEmpty().WithMessage("Street Name cannont be empty")
               .MaximumLength(50).WithMessage("Street Name has to have less than 50 digits")
               .MinimumLength(5).WithMessage("Street Name has to have atleast 2 digits");

            RuleFor(pr => pr.Address.PostalCode)
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
