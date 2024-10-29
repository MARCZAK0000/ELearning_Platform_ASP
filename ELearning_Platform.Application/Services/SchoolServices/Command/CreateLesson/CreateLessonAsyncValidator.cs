using FluentValidation;

namespace ELearning_Platform.Application.Services.SchoolServices.Command.CreateLesson
{
    public class CreateLessonAsyncValidator : AbstractValidator<CreateLessonAsyncCommand>
    {
        public CreateLessonAsyncValidator()
        {
            RuleFor(pr=>pr.LessonDate)
                .NotNull()
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));

            RuleFor(pr => pr.LessonName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(pr=>pr.LessonDescription)
                .NotEmpty()
                .MaximumLength(150);
        }
    }
}
