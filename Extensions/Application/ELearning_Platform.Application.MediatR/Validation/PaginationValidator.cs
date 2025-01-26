using ELearning_Platform.Domain.Models.Models.Pagination;
using FluentValidation;

namespace ELearning_Platform.Application.MediatR.Validation
{
    public class PaginationValidator : AbstractValidator<PaginationModelDto>
    {
        public PaginationValidator()
        {
            RuleFor(pr => pr.PageIndex)
                .GreaterThanOrEqualTo(1);

            RuleFor(pr => pr.PageSize)
                .GreaterThanOrEqualTo(0);

        }
    }
}
