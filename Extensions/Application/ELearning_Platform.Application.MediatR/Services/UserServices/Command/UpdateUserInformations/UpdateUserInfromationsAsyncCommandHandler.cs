using ELearning_Platform.Application.Authorization.Authorization;
using ELearning_Platform.Domain.Core.Repository;
using MediatR;

namespace ELearning_Platform.Application.MediatR.Services.UserServices.Command.UpdateUserInformations
{
    public class UpdateUserInfromationsAsyncCommandHandler(IUserRepository userRepository,
        IUserContext userContext) : IRequestHandler<UpdateUserInformationsAsyncCommand, bool>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUserContext _userContext = userContext;
        public async Task<bool> Handle(UpdateUserInformationsAsyncCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            return await _userRepository.UpdateUserInfomrationsAsync(userID: currentUser.UserID,
                updateUserInformations: request, token: cancellationToken);
        }
    }
}
