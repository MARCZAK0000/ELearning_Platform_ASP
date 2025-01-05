using ELearning_Platform.Application.CustomAttributes;
using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Exceptions;
using ELearning_Platform.Domain.Models.Pagination;
using ELearning_Platform.Domain.Models.SchoolModel;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Response.ClassResponse;
using ELearning_Platform.Domain.Response.Pagination;
using ELearning_Platform.Domain.Response.SchoolResponse;
using ELearning_Platform.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace ELearning_Platform.Infrastructure.Repository
{
    public class SchoolRepository
        (PlatformDb platformDb,
        UserManager<Account> userManager) : ISchoolRepository
    {

        #region Dependecy Properties
        private readonly PlatformDb _platformDb = platformDb;
        private readonly UserManager<Account> _userManager = userManager;
        #endregion

        #region Commands Method
        
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
     
        public async Task<AddStudentToClassResponse> AddStudentToClassAsync(ELearningClass eClass, AddStudentToClassDto addToClass, CancellationToken token)
        {
            var usersToAdd = await _platformDb.UserInformations
                .Include(pr => pr.Account)
                .Where(u => addToClass.UsersToAdd.Contains(u.AccountID))
                .ToListAsync(cancellationToken: token);

            foreach (var item in usersToAdd)
            {
                if (!await _userManager.IsInRoleAsync(item.Account, "student"))
                {
                    usersToAdd.Remove(item);
                }
            }

            if (usersToAdd.Count != addToClass.UsersToAdd.Count)
            {
                throw new NotFoundException("One or more users not found");
            }

            eClass.Students ??= [];

            eClass.Students.AddRange(usersToAdd);
            await _platformDb.SaveChangesAsync(token);
            return new AddStudentToClassResponse()
            {
                AddedUsers = usersToAdd,
                IsSuccess = true,
            };

        }
       
        public async Task<AddStudentToClassResponse> AddUsersToClassSubjectAsync(List<Subject> subjects, IList<string> usersToAdd, CancellationToken token)
        {

            var list = new List<StudentSubject>();
            foreach (var subject in subjects)
            {
                foreach (var user in usersToAdd)
                {
                    list.Add(new StudentSubject()
                    {
                        StudentID = user,
                        SubjectID = subject.SubjectId,
                    });
                }
            }

            await _platformDb.StudentSubjects.AddRangeAsync(list, token);

            await _platformDb.SaveChangesAsync(token);

            return new AddStudentToClassResponse()
            {
                IsSuccess = true,
            };

        }
        
        public async Task<Lesson> CreateLessonAsync(string userId, Subject findSubject, CreateLessonDto createLessonDto, CancellationToken token)
        {

            var lesson = new Lesson
            {
                ClassID = findSubject.ClassID,
                SubjectID = createLessonDto.SubjectID,
                LessonDate = createLessonDto.LessonDate,
                LessonTopic = createLessonDto.LessonDescription,
                LessonDescription = createLessonDto.LessonDescription,
                TeacherID = userId,
            };

            await _platformDb.Lessons.AddAsync(lesson, token);
            await _platformDb.SaveChangesAsync(token);

            return lesson;
        }
        
        public async Task<bool> CreateSubjectAsync(string userID, CreateSubjectDto createSubjectDto, CancellationToken token)
        {

            if (string.IsNullOrEmpty(createSubjectDto.TeacherID))
            {
                var teacherData = await _platformDb.UserInformations
                    .Where(pr => pr.AccountID == userID)
                    .Select(pr => new { pr.FirstName, pr.Surname })
                    .FirstOrDefaultAsync(token) ??
                    throw new NotFoundException("Invalid Teacher");
            }
            else
            {
                var teacherData = await _platformDb.UserInformations
                    .Where(pr => pr.AccountID == createSubjectDto.TeacherID)
                    .Select(pr => new { pr.FirstName, pr.Surname })
                    .FirstOrDefaultAsync(token) ??
                    throw new NotFoundException("Invalid Teacher");

            }

            var subject = new Subject()
            {
                ClassID = createSubjectDto.ClassID,
                Name = createSubjectDto.SubjectName,
                Description = createSubjectDto.SubjectDescription,
                TeacherID = createSubjectDto.TeacherID ?? userID,
            };
            await _platformDb.Subjects.AddAsync(subject, token);
            await _platformDb.SaveChangesAsync(token);

            return true;

        }


        #endregion

        #region Query Method
        public async Task<Pagination<Lesson>> GetLessonsBySubjectAsync(string subjectID, PaginationModelDto paginationModelDto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Lesson?> FindLessonByIDAsync(string lessonID, string subjectID, CancellationToken token)
        {
            
            return await _platformDb
                .Lessons
                .Include(x => x.LessonMaterials)
                .Where(pr => pr.LessonID == lessonID && pr.SubjectID == subjectID)
                .FirstOrDefaultAsync(token);
        }

        public async Task<Subject?> FindSubjectByTeacherIDAsync(string TeacherID, CancellationToken token)
        {
            return await _platformDb
                .Subjects
                .Where(pr => pr.TeacherID == TeacherID)
                .FirstOrDefaultAsync(token);
                
        }

        public async Task<ELearningClass?> FindClassWithStudentsByIdAsync(string id, CancellationToken token)
        {
            
            return await _platformDb.ELearningClasses.
                Where(pr => pr.ELearningClassID == id)
                .Include(pr => pr.Students)
                .FirstOrDefaultAsync(token);

        }

        public async Task<Subject?> FindSubjectByIDAsync(string subjectID, CancellationToken cancellationToken)
        {

            return await _platformDb
                .Subjects
                .Where(pr => pr.SubjectId == subjectID)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<Subject>> FindSubjectByClassIDAsync(string classId, CancellationToken token)
        {
            return await _platformDb
                .Subjects
                .Where(pr => pr.ClassID == classId)
                .ToListAsync(token);
        }

        public async Task<ELearningClass?> FindClassByClassIDAsync(string classId, CancellationToken token)
        {
            return await _platformDb
                .ELearningClasses
                .Where(pr => pr.ELearningClassID == classId)
                .FirstOrDefaultAsync(token);
        }

        public async Task<ELearingClassDto?> FindInformationsAboutClassByClassIDAsync
            (string classID, bool withStudents, bool withSubjecs, bool withTeachers, CancellationToken cancellationToken)
        {
            var baseQuery =
                _platformDb
                .ELearningClasses
                .Where(pr => pr.ELearningClassID == classID);
            if (withStudents)
            {
                baseQuery = baseQuery
                    .Include(pr => pr.Students);
            }
            if (withSubjecs)
            {
                baseQuery = baseQuery
                    .Include(pr=>pr.Subjects);
            }
            if (withTeachers)
            {
                baseQuery =
                    baseQuery
                    .Include(pr => pr.Teachers);
            }

            var result = await baseQuery.Select(pr => new ELearingClassDto
            {
                ELearningClassID = pr.ELearningClassID,
                Name = pr.Name,
                YearOfBeggining = pr.YearOfBeggining,
                YearOfEnding = pr.YearOfEnding,
                Students = pr.Students!.Select(pr => new StudentInformationsDto
                {
                    AccountID = pr.AccountID,
                    FirstName = pr.FirstName,
                    SecondName = pr.SecondName,
                    Surname = pr.Surname,
                }).ToList() ?? new List<StudentInformationsDto>(),
                Subjects = pr.Subjects!.Select(pr => new SubjectInformationsDto
                {
                    Name = pr.Name,
                    Description = pr.Description,
                    TeacherID = pr.TeacherID,
                }).ToList() ?? new List<SubjectInformationsDto>(),
                Teachers = pr.Teachers!.Select(pr => new TeacherInfromationsDto
                {
                    AccountID = pr.AccountID,
                    FirstName = pr.FirstName,
                    SecondName = pr.SecondName,
                    Surname = pr.Surname,
                }).ToList() ?? new List<TeacherInfromationsDto>(),
                ModifiedDate = pr.ModifiedDate
            })
            .FirstOrDefaultAsync(cancellationToken);
            return result;

        } 
        #endregion

    }
}
