using ELearning_Platform.Domain.Core.Repository;
using ELearning_Platform.Domain.Exceptions.Exceptions;
using ELearning_Platform.Domain.Models.Models.Pagination;
using ELearning_Platform.Domain.Models.Response.Pagination;
using ELearning_Platform.Domain.Models.Response.UserReponse;
using FluentValidation;
using MediatR;

namespace ELearning_Platform.Application.MediatR.Services.UserServices.Queries.AllUserInformations
{
    public class GetInfromationsAboutAllUsersAsyncQueryHandler(IValidator<PaginationModelDto> validator, IUserRepository userRepository) :
        IRequestHandler<GetInfromationsAboutAllUsersAsyncQuery, Pagination<GetUserInformationsDto>>
    {

        private readonly IValidator<PaginationModelDto> _validator = validator;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Pagination<GetUserInformationsDto>> Handle(GetInfromationsAboutAllUsersAsyncQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new BadRequestException(string.Join(", ", validationResult.Errors));
            }
            return await _userRepository.GetAllUsersAsync(request, cancellationToken);
        }
    }
}
