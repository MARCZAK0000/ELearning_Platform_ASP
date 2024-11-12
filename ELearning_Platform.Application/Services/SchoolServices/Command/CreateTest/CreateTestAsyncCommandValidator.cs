using ELearning_Platform.Domain.Models.SchoolModel;
using FluentValidation;

namespace ELearning_Platform.Application.Services.SchoolServices.Command.CreateTest
{
    public class CreateTestAsyncCommandValidator : AbstractValidator<CreateTestAsyncCommand> 
    {
        public CreateTestAsyncCommandValidator()
        {
            RuleFor(pr=>pr.StartTime)
                .GreaterThanOrEqualTo(DateTime.Now)
                .WithMessage("Start Time has to be greater or equal than current time");

            RuleFor(pr=>pr.EndTime)
                .GreaterThan(pr=>pr.StartTime)
                .WithMessage("End Time has to be greater than start time");

            RuleFor(pr => pr.TestLevel)
                .IsInEnum();

            RuleFor(pr => pr.SubjectID)
                .NotEmpty();

            RuleFor(pr=>pr.TestName)
                .NotEmpty()
                .MinimumLength(3).WithMessage("Test name lenght has to be greater than 3")
                .MaximumLength(45).WithMessage("Test name lenght has to be lower than 45");

            RuleFor(pr => pr.TestDescription)
                .NotEmpty()
                .MinimumLength(3).WithMessage("Test Description lenght has to be greater than 3")
                .MaximumLength(250).WithMessage("Test Description lenght has to be lower than 250");
        }
    }
}
