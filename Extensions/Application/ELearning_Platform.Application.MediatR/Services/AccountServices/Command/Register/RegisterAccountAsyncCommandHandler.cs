using ELearning_Platform.Domain.Core.Repository;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace ELearning_Platform.Application.MediatR.Services.AccountServices.Command.Register
{
    public class RegisterAccountAsyncCommandHandler(IAccountRepository accountRepository)
        : IRequestHandler<RegisterAccountAsyncCommand>
    {
        private readonly IAccountRepository _accountRepository = accountRepository;

        public async Task Handle(RegisterAccountAsyncCommand request, CancellationToken cancellationToken)
        {
            await _accountRepository.RegisterAccountAsync(registerModelDto: request, cancellationToken: cancellationToken);
        }
    }
}
