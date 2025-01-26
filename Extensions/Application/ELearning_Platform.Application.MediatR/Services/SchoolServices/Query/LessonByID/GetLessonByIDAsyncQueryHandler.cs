using ELearning_Platform.Application.Authorization.Authorization;
using ELearning_Platform.Domain.Core.Repository;
using ELearning_Platform.Domain.Database.Enitities;
using ELearning_Platform.Domain.Models.ErrorResponses;
using ELearning_Platform.Domain.Setttings.Settings;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ELearning_Platform.Application.MediatR.Services.SchoolServices.Query.LessonByID
{
    public class GetLessonByIDAsyncQueryHandler(IUserContext userContext, ISchoolRepository schoolRepository)
        : IRequestHandler<GetLessonByIDAsyncQuery, Results<Ok<Lesson>, NotFound<ProblemDetails>, ForbidHttpResult>>
    {
        private readonly IUserContext _userContext = userContext;
        private readonly ISchoolRepository _schoolRepository = schoolRepository;

        public async Task<Results<Ok<Lesson>, NotFound<ProblemDetails>, ForbidHttpResult>> Handle(GetLessonByIDAsyncQuery request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();

            if (currentUser == null ||
                currentUser.IsInRole(nameof(AuthorizationRole.moderator)))
            {
                return TypedResults.Forbid();
            }
            return await _schoolRepository.FindLessonByIDAsync(request.LessonID, request.SubjectID, cancellationToken)
                is { } result
                    ? TypedResults.Ok(result)
                        : TypedResults.NotFound(ErrorCodesResponse.GenerateErrorResponse(ErrorCode.NotFound, "Invalid LessonID"));
        }
    }
}
