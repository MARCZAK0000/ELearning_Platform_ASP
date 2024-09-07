using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Response.Account;
using MediatR;

namespace ELearning_Platform.Application.Services.AccountServices.Command.RefreshToken
{
    public class RefreshTokenAsyncCommandHandler(IAccountRepository accountRespository) : IRequestHandler<RefreshTokenAsyncCommand, LoginResponse>
    {
        private readonly IAccountRepository _accountRepository = accountRespository;    
        public async Task<LoginResponse> Handle(RefreshTokenAsyncCommand request, CancellationToken cancellationToken)
            => await _accountRepository.RefreshTokenAsync(refreshTokenModelDto: request,cancellationToken: cancellationToken);
    }
}
