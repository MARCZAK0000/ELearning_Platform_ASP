﻿using FluentValidation;

namespace ELearning_Platform.Application.MediatR.Services.SchoolServices.Command.CreateClass
{
    public class CreateClassAsyncCommandValidator : AbstractValidator<CreateClassAsyncCommand>
    {
        public CreateClassAsyncCommandValidator()
        {
            RuleFor(pr => pr.Name)
                .NotEmpty().WithMessage("Name cannot be null or whitespaces");

            RuleFor(pr => pr.YearOfBegining)
                .GreaterThanOrEqualTo(2020)
                .LessThanOrEqualTo(pr => pr.YearOfEnding)
                .Custom((value, context) =>
                {
                    if (value < DateTime.Now.Year)
                    {
                        context.AddFailure("Invalid year");
                    }
                });

            RuleFor(pr => pr.YearOfBegining)
                .LessThanOrEqualTo(pr => pr.YearOfEnding + 5).WithMessage("Invalid message");

        }
    }
}
