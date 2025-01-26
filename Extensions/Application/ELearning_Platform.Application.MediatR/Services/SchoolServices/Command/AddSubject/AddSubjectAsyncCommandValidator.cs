using FluentValidation;

namespace ELearning_Platform.Application.MediatR.Services.SchoolServices.Command.AddSubject
{
    public class AddSubjectAsyncCommandValidator : AbstractValidator<AddSubjectAsyncCommand>
    {
        public AddSubjectAsyncCommandValidator()
        {
            RuleFor(pr => pr.SubjectName)
                .NotEmpty().WithMessage("Name cannot be empty")
                .MaximumLength(250)
                .MinimumLength(3);

            RuleFor(pr => pr.SubjectDescription)
                .NotEmpty();

            RuleFor(pr => pr.ClassID)
                .NotEmpty().WithMessage("Class Id cannot be empty");
        }
    }
}
