using ELearning_Platform.Infrastructure.Authorization;
using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Exceptions;
using ELearning_Platform.Domain.Models.SchoolModel;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Response.ClassResponse;
using ELearning_Platform.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

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
          
            var newClass = new ELearningClass()
            {
                Name = createClass.Name,
                YearOfBeggining = createClass.YearOfBegining,
                YearOfEnding = createClass.YearOfEnding,
            };

            await _platformDb.ELearningClasses.AddAsync(entity: newClass, cancellationToken: token);
            await _platformDb.SaveChangesAsync(cancellationToken: token);

            return new CreateClassResponse()
            {
                IsCreated = true,
                Name = createClass.Name,
            };
        }

        public async Task<AddStudentToClassResponse> AddStudentToClassAsync(AddStudentToClassDto addToClass, CancellationToken token)
        {
            var eClass = await _platformDb
                .ELearningClasses
                .Where(pr => pr.ELearningClassID == addToClass.ClassID)
                .FirstOrDefaultAsync(cancellationToken: token)
                ?? throw new NotFoundException("Class not found");

            foreach (var item in addToClass.UsersToAdd)
            {
                eClass.Students!.Add
                    (await _platformDb.UserInformations.Where(pr => pr.AccountID == item)
                    .FirstOrDefaultAsync(cancellationToken: token) ?? throw new NotFoundException("Student not found"));
            }

            return new AddStudentToClassResponse()
            {
                AddedUsers = addToClass.UsersToAdd,
                ClassName = eClass.Name,
                IsSuccess = true,
            };

        }
    }
}
