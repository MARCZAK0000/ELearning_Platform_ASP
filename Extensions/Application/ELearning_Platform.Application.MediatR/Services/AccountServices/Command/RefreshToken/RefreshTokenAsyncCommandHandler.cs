using ELearning_Platform.Application.Authorization.Authorization;
using ELearning_Platform.Domain.Core.Repository;
using MediatR;

namespace ELearning_Platform.Application.MediatR.Services.AccountServices.Command.RefreshToken
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
