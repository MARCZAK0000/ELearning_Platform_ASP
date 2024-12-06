using ELearning_Platform.Domain.Authorization;
using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.ErrorResponses;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Response.Pagination;
using ELearning_Platform.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ELearning_Platform.Application.Services.ELearningTestServices.Query.GetTestsBySubjectID
{
    public class FindTestBySubjectIDAsyncQueryHandler(IUserContext userContext,
        ISchoolRepository schoolRepository,
        IElearningTestRepository elearningTestRepository) :
        IRequestHandler<FindTestsBySubjectIDAsyncQuery, Results<Ok<Pagination<Test>>, ForbidHttpResult, NotFound<ProblemDetails>>>
    {
        private readonly IUserContext _userContext = userContext;
        private readonly ISchoolRepository _schoolRepository = schoolRepository;
        private readonly IElearningTestRepository _elearningTestRepository = elearningTestRepository;

        public async Task<Results<Ok<Pagination<Test>>, ForbidHttpResult, NotFound<ProblemDetails>>>
            Handle(FindTestsBySubjectIDAsyncQuery request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();

            var subject = await _schoolRepository.FindSubjectByIDAsync(request.ID, cancellationToken);

            if (currentUser.UserID != subject.TeacherID ||
                !currentUser.IsInRole(nameof(AuthorizationRole.moderator)))
            {
                var findUserClass = await _schoolRepository
                    .FindClassWithStudentsByIdAsync(currentUser.UserID, cancellationToken);

                if (findUserClass == null || findUserClass.ELearningClassID != subject.ClassID)
                {
                    return TypedResults.Forbid(ErrorCodesResponse.ForbidError());
                }
            }

            var result = await _elearningTestRepository.
                FindTestsBySubjectIDAsync(request.ID, request.PaginationModelDto, cancellationToken);

            if (result.TotalCount == 0)
            {
                return TypedResults.NotFound(
                    ErrorCodesResponse.GenerateErrorResponse(ErrorCode.NotFound, "Not Found Test for this subjects")
                );
            }

            return TypedResults.Ok(result);
        }
    }
}
