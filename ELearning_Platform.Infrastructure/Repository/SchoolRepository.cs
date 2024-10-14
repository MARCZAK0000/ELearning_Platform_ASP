using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Exceptions;
using ELearning_Platform.Domain.Models.SchoolModel;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Response.ClassResponse;
using ELearning_Platform.Infrastructure.Authorization;
using ELearning_Platform.Infrastructure.BackgroundStrategy;
using ELearning_Platform.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ELearning_Platform.Infrastructure.Repository
{
    public class SchoolRepository
        (IUserContext userContext,
        PlatformDb platformDb,
        BackgroundTask backgroundTask,
        UserManager<Account> userManager) : ISchoolRepository
    {
        private readonly BackgroundTask _backgroundTask = backgroundTask;

        private readonly IUserContext _userContext = userContext;

        private readonly PlatformDb _platformDb = platformDb;

        private readonly UserManager<Account> _userManager = userManager;

        public async Task<CreateClassResponse> CreateClassAsync
            (CreateClassDto createClass, CancellationToken token)
        {

            var newClass = new ELearningClass()
            {
                Name = createClass.Name,
                YearOfBeggining = createClass.YearOfBegining,
                YearOfEnding = createClass.YearOfEnding,
            };

            await _platformDb.ELearningClasses.AddAsync(entity: newClass, cancellationToken: token);
            await _platformDb.SaveChangesAsync(cancellationToken: token);

            return new CreateClassResponse()
            {
                IsCreated = true,
                Name = createClass.Name,
            };
        }

        public async Task<AddStudentToClassResponse> AddStudentToClassAsync(AddStudentToClassDto addToClass, CancellationToken token)
        {
            var eClass = await _platformDb
                .ELearningClasses
                .Where(pr => pr.ELearningClassID == addToClass.ClassID)
                .Select(pr => pr.Students)
                .FirstOrDefaultAsync(cancellationToken: token)
                ?? throw new NotFoundException("Class not found");


            var usersToAdd = await _platformDb.UserInformations
                .Include(pr=>pr.Account)
                .Where(u => addToClass.UsersToAdd.Contains(u.AccountID) 
                    && !_userManager.GetRolesAsync(u.Account).GetAwaiter().GetResult().Contains("student"))
                .ToListAsync(cancellationToken: token);


            if (usersToAdd.Count != addToClass.UsersToAdd.Count)
            {
                throw new NotFoundException("One or more users not found");
            }

            eClass ??= new List<UserInformations>();

            eClass.AddRange(usersToAdd);

            return new AddStudentToClassResponse()
            {
                AddedUsers = addToClass.UsersToAdd,
                IsSuccess = true,
            };

        }
    }
}
