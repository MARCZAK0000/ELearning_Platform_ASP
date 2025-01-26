using ELearning_Platform.Application.Authorization.Authorization;
using ELearning_Platform.Domain.Core.Repository;
using MediatR;

namespace ELearning_Platform.Application.MediatR.Services.UserServices.Command.UpdateImage
{
    public class UpdateOrCreateImageProfileAsyncCommandHandler(
        IUserRepository userRepository, IUserContext userContext) : IRequestHandler<UpdateOrCreateImageProfileAsyncCommand, bool>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUserContext _userContext = userContext;
        public async Task<bool> Handle(UpdateOrCreateImageProfileAsyncCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            return await _userRepository.UpdateOrCreateImageProfile(userID: currentUser.UserID, request.Image, cancellationToken);
        }
    }
}
