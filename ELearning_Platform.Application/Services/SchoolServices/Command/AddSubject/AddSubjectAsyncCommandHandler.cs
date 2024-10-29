using ELearning_Platform.Domain.Authorization;
using ELearning_Platform.Domain.Exceptions;
using ELearning_Platform.Domain.Models.Notification;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Infrastructure.Authorization;
using MediatR;

namespace ELearning_Platform.Application.Services.SchoolServices.Command.AddSubject
{
    public class AddSubjectAsyncCommandHandler(ISchoolRepository schoolRepository, 
        IUserContext userContext) : IRequestHandler<AddSubjectAsyncCommand, bool>
    {
        private readonly ISchoolRepository _schoolRepository = schoolRepository;
        private readonly IUserContext _userContext = userContext;

        public async Task<bool> Handle(AddSubjectAsyncCommand request, CancellationToken cancellationToken)
        {

            var currentUser = _userContext.GetCurrentUser();

            if (currentUser.IsInRole(nameof(AuthorizationRole.student)))
            {
                throw new ForbidenException("Forbiden action");
            }

            if ((currentUser.IsInRole(nameof(AuthorizationRole.moderator))
                || currentUser.IsInRole(nameof(AuthorizationRole.admin))) && string.IsNullOrEmpty(request.TeacherID))
            {
                throw new BadRequestException("Invalid TeacherID");
            }

            if (!await _schoolRepository.CreateSubjectAsync(currentUser.UserID, request, cancellationToken))
            {
                throw new BadRequestException("Cannot create subject");
            }
            return true;
        }
    }
}
