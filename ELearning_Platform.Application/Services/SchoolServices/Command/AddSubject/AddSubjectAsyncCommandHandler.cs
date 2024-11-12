using ELearning_Platform.Domain.Authorization;
using ELearning_Platform.Domain.Exceptions;
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

            if (currentUser.IsInRole(nameof(AuthorizationRole.student).ToLower()))
            {
                throw new ForbidenException("Forbiden action");
            }

            if ((currentUser.IsInRole(nameof(AuthorizationRole.moderator))
                || currentUser.IsInRole(nameof(AuthorizationRole.admin))) && string.IsNullOrEmpty(request.TeacherID))
            {
                throw new NotFoundException("Invalid TeacherID");
            }

            _ = await _schoolRepository.FindClassWithStudentsByIdAsync(request.ClassID, cancellationToken)
                ?? throw new BadRequestException("Invalid ClassID");
            await _schoolRepository.CreateSubjectAsync(currentUser.UserID, request, cancellationToken);
            return true;
        }
    }
}
