using AutoMapper;
using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Response.UserReponse;
using MediatR;

namespace ELearning_Platform.Infrastructure.Services.UserServices.Queries.Informations
{
    public class GetUserInformationsAsyncQueryHandler
        (IUserRepository userRepository) : IRequestHandler<GetUserInformationsAsyncQuery, GetUserInformationsDto>
    {
        private readonly IUserRepository _userRepository = userRepository;
       

        public async Task<GetUserInformationsDto> Handle(GetUserInformationsAsyncQuery request, CancellationToken cancellationToken)
            => await _userRepository.GetUserInformationsAsync(token: cancellationToken);
    }
}
