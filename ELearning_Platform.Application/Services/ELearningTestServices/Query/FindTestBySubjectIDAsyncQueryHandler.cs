using ELearning_Platform.Domain.Authorization;
using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Exceptions;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Response.Pagination;
using ELearning_Platform.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace ELearning_Platform.Application.Services.ELearningTestServices.Query
{
    public class FindTestBySubjectIDAsyncQueryHandler(IUserContext userContext, 
        ISchoolRepository schoolRepository, 
        IElearningTestRepository elearningTestRepository) :
        IRequestHandler<FIndTestsBySubjectIDAsyncQuery, Results<Ok<Pagination<Test>>, ForbidHttpResult, NotFound<ProblemDetails>>>
    {
        private readonly IUserContext _userContext = userContext;
        private readonly ISchoolRepository _schoolRepository = schoolRepository;
        private readonly IElearningTestRepository _elearningTestRepository = elearningTestRepository;

        public async Task<Results<Ok<Pagination<Test>>, ForbidHttpResult, NotFound<ProblemDetails>>>
            Handle(FIndTestsBySubjectIDAsyncQuery request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            
            var subject = await _schoolRepository.FindSubjectByIDAsync(request.ID, cancellationToken);

            if(currentUser.UserID != subject.TeacherID || 
                !currentUser.IsInRole(nameof(AuthorizationRole.moderator)))
            {
                var findUserClass = await _schoolRepository
                    .FindClassWithStudentsByIdAsync(currentUser.UserID, cancellationToken);

                if(findUserClass == null || findUserClass.ELearningClassID !=subject.ClassID)
                {
                    return TypedResults.Forbid();
                }
            } 

            var result = await _elearningTestRepository.
                FindTestsBySubjectIDAsync(request.ID, request.PaginationModelDto, cancellationToken);

            if(result.TotalCount == 0)
            {
                return TypedResults.NotFound(new ProblemDetails
                {
                    Status = (int)HttpStatusCode.NotFound,
                    Detail = "Not Found Test for this subjects",
                    Title = "Not Found"
                });
            }

            return TypedResults.Ok(result);
        }
    }
}
