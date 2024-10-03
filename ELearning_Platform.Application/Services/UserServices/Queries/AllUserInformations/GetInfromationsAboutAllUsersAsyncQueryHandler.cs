using AutoMapper;
using ELearning_Platform.Domain.Exceptions;
using ELearning_Platform.Domain.Models.Pagination;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Response.Pagination;
using ELearning_Platform.Domain.Response.UserReponse;
using ELearning_Platform.Infrastructure.Services.UserServices.Queries.AllUserInformations;
using FluentValidation;
using MediatR;

namespace ELearning_Platform.Application.Services.UserServices.Queries.AllUserInformations
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
