using ELearning_Platform.Domain.Authorization;
using ELearning_Platform.Domain.Models.Notification;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Infrastructure.Authorization;
using MediatR;

namespace ELearning_Platform.Infrastructure.Services.SchoolServices.Command.AddToClass
{
    public class AddToClassAsyncCommandHanlder
        (ISchoolRepository schoolRepository,
        INotificaitonRepository notificaitonRepository,
        IUserContext userContext)
        : IRequestHandler<AddToClassAsyncCommand, bool>
    {
        private readonly ISchoolRepository _schoolRepository = schoolRepository;
        private readonly INotificaitonRepository _notificaitonRepository = notificaitonRepository;
        private readonly IUserContext _userContext = userContext;
        public async Task<bool> Handle(AddToClassAsyncCommand request, CancellationToken cancellationToken)
        {

            var currentUser = _userContext.GetCurrentUser();

            if (!currentUser.IsInRole(nameof(AuthorizationRole.moderator))
                && !currentUser.IsInRole(nameof(AuthorizationRole.admin))
                    && !currentUser.IsInRole(nameof(AuthorizationRole.headTeacher)))
            {
                return false;
            }

            var result = await _schoolRepository.AddStudentToClassAsync(request, cancellationToken);
            if (!result.IsSuccess) return false;

            var notifications = new List<CreateNotificationDto>();

            foreach (var item in result.AddedUsers)
            {
                notifications.Add(new CreateNotificationDto
                {
                    Title = $"Add to class: ${item.ClassID}",
                    Describtion = "You have been add to class ",
                    ReciverID = item.AccountID,
                    EmailAddress = item.EmailAddress,
                    SenderID = currentUser.UserID,
                });
            }

            await _notificaitonRepository
                .CreateMoreThanOneNotificationAsync(
                    currentUser: (currentUser.EmailAddress, currentUser.UserID),notifications, cancellationToken);

            return true;
        }
    }
}
