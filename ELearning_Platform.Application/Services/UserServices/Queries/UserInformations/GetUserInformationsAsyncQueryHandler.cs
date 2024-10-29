using AutoMapper;
using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Response.UserReponse;
using ELearning_Platform.Infrastructure.Authorization;
using MediatR;

namespace ELearning_Platform.Infrastructure.Services.UserServices.Queries.Informations
{
    public class GetUserInformationsAsyncQueryHandler
        (IUserRepository userRepository, IUserContext userContext) 
        : IRequestHandler<GetUserInformationsAsyncQuery, GetUserInformationsDto>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUserContext _userContext = userContext;


        public async Task<GetUserInformationsDto> Handle(GetUserInformationsAsyncQuery request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            return await _userRepository.GetUserInformationsAsync(userID:currentUser.UserID, token: cancellationToken);
        }
    }
}
