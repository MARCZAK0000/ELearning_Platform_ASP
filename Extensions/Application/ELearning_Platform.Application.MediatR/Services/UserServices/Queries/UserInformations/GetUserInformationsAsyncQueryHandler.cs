using ELearning_Platform.Application.Authorization.Authorization;
using ELearning_Platform.Domain.Core.Repository;
using ELearning_Platform.Domain.Models.Response.UserReponse;
using MediatR;

namespace ELearning_Platform.Application.MediatR.Services.UserServices.Queries.UserInformations
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
            return await _userRepository.GetUserInformationsAsync(userID: currentUser.UserID, token: cancellationToken);
        }
    }
}
