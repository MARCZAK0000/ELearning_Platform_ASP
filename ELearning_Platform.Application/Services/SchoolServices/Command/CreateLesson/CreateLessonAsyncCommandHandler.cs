using ELearning_Platform.Domain.Authorization;
using ELearning_Platform.Domain.Models.Notification;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Infrastructure.Authorization;
using MediatR;

namespace ELearning_Platform.Application.Services.SchoolServices.Command.CreateLesson
{
    public class CreateLessonAsyncCommandHandler(ISchoolRepository schoolRepository,
        INotificaitonRepository notificaitonRepository
        , IUserContext userContext, ILessonMaterialsRepository materialsRepository) : IRequestHandler<CreateLessonAsyncCommand, bool>
    {
        private readonly ISchoolRepository _schoolRepository = schoolRepository;

        private readonly INotificaitonRepository _notificaitonRepository = notificaitonRepository;

        private readonly ILessonMaterialsRepository _materialsRepository = materialsRepository;

        private readonly IUserContext _userContext = userContext;

        /// <summary>
        /// Creat Lesson 
        /// </summary>
        /// <param name="request">Lesson Parameters</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Method returs <strong>bool</strong></returns>
        public async Task<bool> Handle(CreateLessonAsyncCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            var subjectInfo = await _schoolRepository.FindSubjectByIDAsync(request.SubjectID, cancellationToken);

            if (subjectInfo == null
                || subjectInfo.TeacherID != currentUser.UserID
                    || !currentUser.IsInRole(nameof(AuthorizationRole.moderator))) return false;

            var result = await _schoolRepository.CreateLessonAsync(
                userID: currentUser.IsInRole(nameof(AuthorizationRole.moderator))
                ? subjectInfo.TeacherID : currentUser.UserID, subject: subjectInfo,
                createLessonDto: request, token: cancellationToken);

            if (result is { }) return false;

            await _materialsRepository.AddLessonMaterialsAsync(request.Materials, result.LessonID.ToString(), cancellationToken);

            var currentClass = await _schoolRepository.
                FindClassByIdAsync(id: subjectInfo.ClassID.ToString(),
                    token: cancellationToken);

            if (currentClass is null || currentClass.Students == null) return true;

            var notifications = new List<CreateNotificationDto>();
            foreach (var item in currentClass.Students)
            {
                notifications.Add(new CreateNotificationDto()
                {
                    Title = request.LessonName,
                    Describtion = $"New Lesson: {request.LessonDescription}\r\n" +
                    $"Date: {request.LessonDate}",
                    EmailAddress = item.EmailAddress,
                    ReciverID = item.AccountID,
                    SenderID = currentUser.UserID,
                });
            }

            await _notificaitonRepository
                 .CreateMoreThanOneNotificationAsync(
                     currentUser: (currentUser.EmailAddress, currentUser.UserID), notifications, cancellationToken);

            return true;

        }
    }
}
