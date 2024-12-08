using ELearning_Platform.Domain.Repository;
using MediatR;

namespace ELearning_Platform.Application.Services.AccountServices.Command.SignOut
{
    public class SignOutAccountAsyncCommandHandler(ITokenRepository tokenRepository) : IRequestHandler<SignOutAccountAsyncCommand, bool>
    {
        private readonly ITokenRepository _tokenRepository = tokenRepository;

        public Task<bool> Handle(SignOutAccountAsyncCommand request, CancellationToken cancellationToken)
        {
            _tokenRepository.RemoveCookies();
            return Task.FromResult(true);
        }
    }
}
