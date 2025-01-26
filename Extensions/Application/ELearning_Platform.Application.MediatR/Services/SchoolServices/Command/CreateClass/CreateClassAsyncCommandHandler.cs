using ELearning_Platform.Application.Authorization.Authorization;
using ELearning_Platform.Domain.Core.Repository;
using ELearning_Platform.Domain.Models.ErrorResponses;
using ELearning_Platform.Domain.Models.Response.ClassResponse;
using ELearning_Platform.Domain.Setttings.Settings;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ELearning_Platform.Application.MediatR.Services.SchoolServices.Command.CreateClass
{
    public class CreateClassAsyncCommandHandler(ISchoolRepository schoolRepository, IUserContext userContext)
        : IRequestHandler<CreateClassAsyncCommand, Results<Ok<CreateClassResponse>, ValidationProblem, ForbidHttpResult>>
    {
        private readonly ISchoolRepository _schoolRepository = schoolRepository;
        private readonly IUserContext _userContext = userContext;

        public async Task<Results<Ok<CreateClassResponse>, ValidationProblem, ForbidHttpResult>>
            Handle(CreateClassAsyncCommand request, CancellationToken cancellationToken)
        {
            var user = _userContext.GetCurrentUser();
            if (!user.IsInRole(nameof(AuthorizationRole.moderator)) &&
                    !user.IsInRole(nameof(AuthorizationRole.admin)))
            {
                return TypedResults.Forbid(ErrorCodesResponse.ForbidError());
            }

            var result = await _schoolRepository.CreateClassAsync(request, cancellationToken);

            return result.IsCreated ? TypedResults.Ok(result) :
                TypedResults.ValidationProblem(ErrorCodesResponse.ValidationProblemResponse("Cannot Add Class"));
        }
    }
}
