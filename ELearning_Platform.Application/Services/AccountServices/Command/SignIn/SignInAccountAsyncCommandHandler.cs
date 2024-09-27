using ELearning_Platform.Domain.Repository;
using MediatR;


namespace ELearning_Platform.Application.Services.AccountServices.Command.SignIn
{
    public class SignInAsyncCommandHandler(IAccountRepository accountRepository, ITokenRepository tokenRepository)
        : IRequestHandler<SignInAsyncCommand, bool>
    {
        private readonly IAccountRepository _accountRepository = accountRepository;

        private readonly ITokenRepository _tokenRepository = tokenRepository;

        public async Task<bool> Handle(SignInAsyncCommand request, CancellationToken cancellationToken)
        {
            var result = await _accountRepository.SignInAsync(loginModelDto: request, cancellationToken: cancellationToken);
            _tokenRepository.SetCookiesInsideResponse(result.TokenModelDto!);
            return true;
        }
    }
}