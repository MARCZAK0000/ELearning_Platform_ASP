using ELearning_Platform.Domain.Authorization;
using ELearning_Platform.Domain.ErrorResponses;
using ELearning_Platform.Domain.Exceptions;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ELearning_Platform.Application.Services.SchoolServices.Command.AddSubject
{
    public class AddSubjectAsyncCommandHandler(ISchoolRepository schoolRepository,
        IUserContext userContext) : IRequestHandler<AddSubjectAsyncCommand, Results<Ok<bool>, ForbidHttpResult, NotFound<ProblemDetails>>>
    {
        private readonly ISchoolRepository _schoolRepository = schoolRepository;
        private readonly IUserContext _userContext = userContext;

        public async Task<Results<Ok<bool>, ForbidHttpResult, NotFound<ProblemDetails>>> Handle(AddSubjectAsyncCommand request, CancellationToken cancellationToken)
        {

            var currentUser = _userContext.GetCurrentUser();

            if (currentUser.IsInRole(nameof(AuthorizationRole.moderator))
                || currentUser.IsInRole(nameof(AuthorizationRole.student))
                    || currentUser.IsInRole(nameof(AuthorizationRole.admin)))
            {
                return TypedResults.Forbid(ErrorCodesResponse.ForbidError());
            }

            var classs = await _schoolRepository.FindClassWithStudentsByIdAsync(request.ClassID, cancellationToken);
            if(classs == null)
            {
                return TypedResults.NotFound(ErrorCodesResponse.GenerateErrorResponse(ErrorCode.NotFound, "Invalid ClassID"));
            }
                
            await _schoolRepository.CreateSubjectAsync(currentUser.UserID, request, cancellationToken);
            return TypedResults.Ok(true);
        }
    }
}
