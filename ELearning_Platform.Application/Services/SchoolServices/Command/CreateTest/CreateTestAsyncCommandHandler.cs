using ELearning_Platform.Domain.Authorization;
using ELearning_Platform.Domain.Models.Notification;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ELearning_Platform.Application.Services.SchoolServices.Command.CreateTest
{
    public class CreateTestAsyncCommandHandler
        (IUserContext userContext, ISchoolRepository schoolRepository, 
        IElearningTestRepository testRepository, 
        INotificaitonRepository notificaitonRepository) :
        IRequestHandler<CreateTestAsyncCommand, Results<Ok, BadRequest, ForbidHttpResult>>
    {
        private readonly IUserContext _userContext = userContext;
        private readonly ISchoolRepository _schoolRepository = schoolRepository;
        private readonly IElearningTestRepository _testRepository = testRepository;
        private readonly INotificaitonRepository _notificaitonRepository = notificaitonRepository;


        public async Task<Results<Ok, BadRequest, ForbidHttpResult>> Handle(CreateTestAsyncCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();

            var getSubject = await _schoolRepository.FindSubjectByIDAsync(request.SubjectID, cancellationToken);

            if (getSubject is null || 
                (getSubject.TeacherID != currentUser.UserID && 
                    currentUser.IsInRole(nameof(AuthorizationRole.moderator))))
            {
                return TypedResults.Forbid();
            }

            var result = await _testRepository
                .CreateTestAsync(request.TeacherID??currentUser.UserID, request, cancellationToken);


            var findClass = await _schoolRepository.FindClassByIdAsync(getSubject.ClassID.ToString(), cancellationToken);

            var notifications = new List<CreateNotificationDto>();

            if(findClass is null || findClass.Students is null)
            {
                return TypedResults.Ok();
            }

            foreach (var item in findClass.Students)
            {
                notifications.Add(new CreateNotificationDto()
                {
                    Title = result.TestName,
                    Describtion = $"New Lesson: {request.TestDescription}\r\n" +
                    $"Date: {request.StartTime.Date}\r\n" +
                    $"Start Time: {request.StartTime} \r\n" +
                    $"End Time: {request.EndTime}",
                    EmailAddress = item.EmailAddress,
                    ReciverID = item.AccountID,
                    SenderID = currentUser.UserID,
                });
            }

            await _notificaitonRepository
                .CreateMoreThanOneNotificationAsync(
                    currentUser: (currentUser.EmailAddress, currentUser.UserID), notifications, cancellationToken);
            return TypedResults.Ok();   
        }
    }
}
