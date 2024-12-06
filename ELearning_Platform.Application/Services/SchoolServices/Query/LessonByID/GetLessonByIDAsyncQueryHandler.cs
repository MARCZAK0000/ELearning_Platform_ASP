using ELearning_Platform.Domain.Authorization;
using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.ErrorResponses;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ELearning_Platform.Application.Services.SchoolServices.Query.LessonByID
{
    public class GetLessonByIDAsyncQueryHandler(IUserContext userContext, ISchoolRepository schoolRepository) 
        : IRequestHandler<GetLessonByIDAsyncQuery, Results<Ok<Lesson>, NotFound<ProblemDetails>,ForbidHttpResult>>
    {
        private readonly IUserContext _userContext = userContext;
        private readonly ISchoolRepository _schoolRepository = schoolRepository;

        public async Task<Results<Ok<Lesson>, NotFound<ProblemDetails> ,ForbidHttpResult>> Handle(GetLessonByIDAsyncQuery request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();

            if (currentUser == null ||
                currentUser.IsInRole(nameof(AuthorizationRole.moderator)))
            {
                return TypedResults.Forbid();
            }
            return (await _schoolRepository.FindLessonByIDAsync(request.LessonID, request.SubjectID, cancellationToken))
                is { } result
                    ? TypedResults.Ok(result)
                        : TypedResults.NotFound(ErrorCodesResponse.GenerateErrorResponse(ErrorCode.NotFound, "Invalid LessonID"));
        }
    }
}
