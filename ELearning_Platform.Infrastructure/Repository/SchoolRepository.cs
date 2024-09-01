using ELearning_Platform.Application.Authorization;
using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Exceptions;
using ELearning_Platform.Domain.Models.SchoolModel;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Response.ClassResponse;
using ELearning_Platform.Infrastructure.Database;

namespace ELearning_Platform.Infrastructure.Repository
{
    public class SchoolRepository
        (IUserContext userContext, PlatformDb platformDb) : ISchoolRepository
    {
        private readonly IUserContext _userContext = userContext;

        private readonly PlatformDb _platformDb = platformDb;

        public async Task<CreateClassResponse> CreateClassAsync
            (CreateClassDto createClass, CancellationToken token)
        {
            var currentUser = _userContext.GetCurrentUser();

            if (currentUser.Roles.Any(pr => pr.Contains("student")))
            {
                throw new ForbidenException("Forbiden");
            }

            var newClass = new ELearningClass()
            {
                Name = createClass.Name,
                YearOfBeggining = createClass.YearOfBegging,
                YearOfEnded = createClass.YearOfEnd,
            };

            await _platformDb.ELearningClasses.AddAsync(entity: newClass, cancellationToken: token);
            await _platformDb.SaveChangesAsync(cancellationToken: token);

            return new CreateClassResponse()
            {
                IsCreated = true,
                Name = createClass.Name,
            };
        }
    }
}
