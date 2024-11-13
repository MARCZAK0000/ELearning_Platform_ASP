using ELearning_Platform.Infrastructure.Authorization;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Response.ClassResponse;
using MediatR;
using ELearning_Platform.Domain.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using ELearning_Platform.Infrastructure.Services.SchoolServices.Command.CreateClass;

namespace ELearning_Platform.Application.Services.SchoolServices.Command.CreateClass
{
    public class CreateClassAsyncCommandHandler(ISchoolRepository schoolRepository, IUserContext userContext)
        : IRequestHandler<CreateClassAsyncCommand, Results<Ok<CreateClassResponse>, ValidationProblem,ForbidHttpResult>>
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
                return TypedResults.Forbid();
            }

            var result = await _schoolRepository.CreateClassAsync(request, cancellationToken);

            return result.IsCreated? TypedResults.Ok(result): 
                TypedResults.ValidationProblem(new Dictionary<string, string[]> 
                { 
                    { "error", ["Cannot Add Class"] } 
                });
        }
    }
}
