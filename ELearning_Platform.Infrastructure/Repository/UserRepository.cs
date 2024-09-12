using ELearning_Platform.Application.Authorization;
using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Exceptions;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace ELearning_Platform.Infrastructure.Repository
{
    public class UserRepository(PlatformDb platformDb, IUserContext userContext) : IUserRepository
    {
        private readonly PlatformDb _platformDb = platformDb;
        private readonly IUserContext _userContext = userContext;

        public async Task<UserInformations> GetUserInformationsAsync(CancellationToken token)
        {
            var currentUser = _userContext.GetCurrentUser();

            var informations = await _platformDb
                .UserInformations
                .Include(pr=>pr.Address)
                .Where(pr=>pr.AccountID == currentUser.UserID)
                .FirstOrDefaultAsync(token)??
                throw new InternalServerErrorException("Something went wrong");

            return informations;
        }

        public async Task<List<UserInformations>> GetAllUsersAsync(CancellationToken token)
        {
            _ = _userContext.GetCurrentUser();
            return await _platformDb.UserInformations.ToListAsync(cancellationToken: token);
        }

    }
}
