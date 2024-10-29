using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Infrastructure.Authorization;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning_Platform.Application.Services.AccountServices.Command.RefreshToken
{
    public class RefreshTokenAsyncCommandHandler(IAccountRepository accountRepository, 
        ITokenRepository tokenRepository,
        IUserContext userContext) : IRequestHandler<RefreshTokenAsyncCommand, bool>
    {   
        private readonly IAccountRepository _accountRepository = accountRepository;
        private readonly ITokenRepository _tokenRepository = tokenRepository;
        private readonly IUserContext _userContext = userContext;
        public async Task<bool> Handle(RefreshTokenAsyncCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            var refreshToken = _tokenRepository.GetRefreshTokenFromContext();
            var result = await _accountRepository.RefreshTokenAsync(currentUser.UserID, refreshToken, cancellationToken);
            _tokenRepository.SetCookiesInsideResponse(result.TokenModelDto!);
            return true;
        }
    }
}
