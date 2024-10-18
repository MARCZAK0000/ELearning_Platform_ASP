using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Exceptions;
using ELearning_Platform.Domain.Models.Notification;
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
        (PlatformDb platformDb,
        UserManager<Account> userManager, INotificaitonRepository notificaitonRepository) : ISchoolRepository
    {
        private readonly PlatformDb _platformDb = platformDb;

        private readonly UserManager<Account> _userManager = userManager;

        private readonly INotificaitonRepository _notificaitonRepository = notificaitonRepository;

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
                    && _userManager.GetRolesAsync(u.Account).GetAwaiter().GetResult().Contains("student"))
                .ToListAsync(cancellationToken: token);

            if (usersToAdd.Count != addToClass.UsersToAdd.Count)
            {
                throw new NotFoundException("One or more users not found");
            }

            eClass ??= new List<UserInformations>();

            eClass.AddRange(usersToAdd);

            await _platformDb.SaveChangesAsync(token);

            var notifications = new List<CreateNotificationDto>();

            foreach (var item in usersToAdd)
            {
                notifications.Add(new CreateNotificationDto
                {
                    Title = nameof(this.AddStudentToClassAsync),
                    Describtion = "Add to Class",
                    ReciverID = item.AccountID,
                    EmailAddress = item.EmailAddress,
                });
            }

            await _notificaitonRepository.CreateMoreThanOneNotificationAsync(notifications, token);
            return new AddStudentToClassResponse()
            {
                AddedUsers = addToClass.UsersToAdd,
                IsSuccess = true,
            };

        }
    }
}
