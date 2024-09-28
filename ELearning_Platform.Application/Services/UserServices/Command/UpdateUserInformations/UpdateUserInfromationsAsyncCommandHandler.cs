using ELearning_Platform.Domain.Repository;
using MediatR;

namespace ELearning_Platform.Application.Services.UserServices.Command.UpdateUserInformations
{
    public class UpdateUserInfromationsAsyncCommandHandler(IUserRepository userRepository) : IRequestHandler<UpdateUserInformationsAsyncCommand, bool>
    {
        private readonly IUserRepository _userRepository = userRepository;
        public async Task<bool> Handle(UpdateUserInformationsAsyncCommand request, CancellationToken cancellationToken)
            => await _userRepository.UpdateUserInfomrationsAsync(updateUserInformations: request, token: cancellationToken);
    }
}
