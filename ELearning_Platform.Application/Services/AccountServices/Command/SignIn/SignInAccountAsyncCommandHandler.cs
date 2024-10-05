using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Response.AccountResponse;
using MediatR;


namespace ELearning_Platform.Application.Services.AccountServices.Command.SignIn
{
    public class SignInAsyncCommandHandler(IAccountRepository accountRepository, ITokenRepository tokenRepository)
        : IRequestHandler<SignInAsyncCommand, LoginResponse>
    {
        private readonly IAccountRepository _accountRepository = accountRepository;

        private readonly ITokenRepository _tokenRepository = tokenRepository;

        public async Task<LoginResponse> Handle(SignInAsyncCommand request, CancellationToken cancellationToken)
        {
            var result = await _accountRepository.SignInAsync(loginModelDto: request, cancellationToken: cancellationToken);
            _tokenRepository.SetCookiesInsideResponse(result.TokenModelDto!);
            result.TokenModelDto = null;
            return result;
        }
    }
}