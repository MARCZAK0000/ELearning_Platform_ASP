using ELearning_Platform.Domain.Models.Pagination;
using ELearning_Platform.Domain.Order;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning_Platform.Application.Validation
{
    public class PaginationValidator: AbstractValidator<PaginationModelDto> 
    {
        public PaginationValidator()
        {
            RuleFor(pr => pr.PageIndex)
                .GreaterThanOrEqualTo(1);

            RuleFor(pr => pr.PageSize)
                .GreaterThanOrEqualTo(0);

            RuleFor(pr => pr.OrderBy)
                .IsInEnum();
            
        }
    }
}
