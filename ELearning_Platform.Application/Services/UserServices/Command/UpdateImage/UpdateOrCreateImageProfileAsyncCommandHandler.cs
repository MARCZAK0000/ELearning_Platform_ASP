using ELearning_Platform.Domain.Repository;
using MediatR;

namespace ELearning_Platform.Application.Services.UserServices.Command.UpdateImage
{
    public class UpdateOrCreateImageProfileAsyncCommandHandler(IUserRepository userRepository) : IRequestHandler<UpdateOrCreateImageProfileAsyncCommand, bool>
    {
        private readonly IUserRepository _userRepository = userRepository;
        public async Task<bool> Handle(UpdateOrCreateImageProfileAsyncCommand request, CancellationToken cancellationToken)
            => await _userRepository.UpdateOrCreateImageProfile(request.Image, cancellationToken);
    }
}
