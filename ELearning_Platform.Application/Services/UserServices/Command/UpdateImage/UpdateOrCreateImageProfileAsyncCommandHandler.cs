using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Infrastructure.Authorization;
using MediatR;

namespace ELearning_Platform.Application.Services.UserServices.Command.UpdateImage
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
