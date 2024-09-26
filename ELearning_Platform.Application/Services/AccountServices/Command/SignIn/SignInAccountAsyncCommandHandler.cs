using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ELearning_Platform.Application.Services.AccountServices.Command.SignIn
{
    public class SignInAsyncCommandHandler(IAccountRepository accountRepository, ITokenRepository tokenRepository)
        : IRequestHandler<SignInAsyncCommand, Results<Ok<bool>, UnauthorizedHttpResult>>
    {
        private readonly IAccountRepository _accountRepository = accountRepository;

        private readonly ITokenRepository _tokenRepository = tokenRepository;

        public async Task<Results<Ok<bool>, UnauthorizedHttpResult>> Handle(SignInAsyncCommand request, CancellationToken cancellationToken)
        {
            var result = await _accountRepository.SignInAsync(loginModelDto: request, cancellationToken: cancellationToken);
            if (!result.Success.Succeeded)
            {
                return TypedResults.Unauthorized();
            }
            _tokenRepository.SetCookiesInsideResponse(result.TokenModelDto!);
            return TypedResults.Ok(true);
        }
    }
}