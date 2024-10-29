using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Infrastructure.Authorization;
using MediatR;

namespace ELearning_Platform.Application.Services.SchoolServices.Command.CreateLesson
{
    public class CreateLessonAsyncCommandHandler(ISchoolRepository schoolRepository, 
        INotificaitonRepository notificaitonRepository
        , IUserContext userContext) : IRequestHandler<CreateLessonAsyncCommand, bool>
    {
        private readonly ISchoolRepository _schoolRepository = schoolRepository;

        private readonly INotificaitonRepository _notificaitonRepository = notificaitonRepository;

        private readonly IUserContext _userContext = userContext;

        public Task<bool> Handle(CreateLessonAsyncCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            throw new NotImplementedException();    
        }
    }
}
