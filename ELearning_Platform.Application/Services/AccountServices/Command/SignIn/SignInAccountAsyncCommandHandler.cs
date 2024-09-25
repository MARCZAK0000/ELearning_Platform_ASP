using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Response.Account;
using MediatR;

namespace ELearning_Platform.Application.Services.AccountServices.Command.SignIn
{
    public class SignInAsyncCommandHandler(IAccountRepository accountRepository)
        : IRequestHandler<SignInAsyncCommand, LoginResponse>
    {
        private readonly IAccountRepository _accountRepository = accountRepository;

        public async Task<LoginResponse> Handle(SignInAsyncCommand request, CancellationToken cancellationToken)
        {
            var result = await _accountRepository.SignInAsync(loginModelDto: request, cancellationToken: cancellationToken);

            return result;
        }
    }
}