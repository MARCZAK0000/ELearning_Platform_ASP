using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Exceptions;
using ELearning_Platform.Domain.Models.Notification;
using ELearning_Platform.Domain.Models.SchoolModel;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Response.ClassResponse;
using ELearning_Platform.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ELearning_Platform.Infrastructure.Repository
{
    public class SchoolRepository
        (PlatformDb platformDb,
        UserManager<Account> userManager, INotificaitonRepository notificaitonRepository,
         ILessonMaterialsRepository lessonMaterialsRepository) : ISchoolRepository
    {
        private readonly PlatformDb _platformDb = platformDb;

        private readonly UserManager<Account> _userManager = userManager;

        private readonly INotificaitonRepository _notificaitonRepository = notificaitonRepository;

        private readonly ILessonMaterialsRepository _lessonMaterialsRepository = lessonMaterialsRepository;

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
                .Select(pr => pr.Students)
                .FirstOrDefaultAsync(cancellationToken: token)
                ?? throw new NotFoundException("Class not found");

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

            eClass ??= new List<UserInformations>();

            eClass.AddRange(usersToAdd);

            await _platformDb.SaveChangesAsync(token);

            return new AddStudentToClassResponse()
            {
                AddedUsers = usersToAdd,
                IsSuccess = true,
            };

        }

        public async Task<bool> CreateLessonAsync(string userId, Subject findSubject, CreateLessonDto createLessonDto, CancellationToken token)
        {

            if (Guid.TryParse(createLessonDto.SubjectID, out var subjectID))
            {
                throw new BadRequestException("Invalid ClassID");
            }

            findSubject.Lessons ??= [];

            var lesson = new Lesson
            {
                ClassID = findSubject.ClassID,
                SubjectID = subjectID,
                LessonDate = createLessonDto.LessonDate,
                LessonTopic = createLessonDto.LessonDescription,
                LessonDescription = createLessonDto.LessonDescription,
                TeacherID = userId,
            };

            var lessonMaterials = new List<LessonMaterials>();

            foreach (var item in createLessonDto.Materials)
            {
                lessonMaterials.Add(new LessonMaterials()
                {
                    LessonID = lesson.LessonID,
                    Name = $"{lesson.LessonID}_{item.FileName}_{lessonMaterials.Count}",
                    Type = Path.GetExtension(item.FileName),
                });
            }
            lesson.LessonMaterials = lessonMaterials;
            await _platformDb.Lessons.AddAsync(lesson, token);
            await _platformDb.SaveChangesAsync(token);


            await _lessonMaterialsRepository.AddLessonMaterialsAsync(createLessonDto.Materials, lesson.LessonID.ToString(), token);

            var findClass = await _platformDb
                .ELearningClasses
                .Include(pr => pr.Students)
                .Where(c => c.ELearningClassID == findSubject.ClassID)
                .FirstOrDefaultAsync(token);

            if (findClass != null && findClass.Students != null)
            {

                var notifications = new List<CreateNotificationDto>();
                foreach (var item in findClass.Students)
                {
                    notifications.Add(new CreateNotificationDto()
                    {
                        Title = createLessonDto.LessonName,
                        Describtion = $"New Lesson: {createLessonDto.LessonDescription}\r\n" +
                        $"Date: {createLessonDto.LessonDate}",
                        EmailAddress = item.EmailAddress,
                        ReciverID = item.AccountID,
                        SenderID = item.AccountID,
                    });
                }
                await _notificaitonRepository.CreateMoreThanOneNotificationAsync(notifications, token);

            }

            return true;
        }

        public async Task<bool> CreateSubjectAsync(string userID, CreateSubjectDto createSubjectDto, CancellationToken token)
        {
            
            if (!Guid.TryParse(createSubjectDto.ClassID, out Guid classID))
            {
                throw new BadRequestException("Invalid class name");
            }

            (string firstName, string surname) teacherInfo;

            if (string.IsNullOrEmpty(createSubjectDto.TeacherID))
            {
                var teacherData = await _platformDb.UserInformations
                    .Where(pr => pr.AccountID == userID)
                    .Select(pr => new { pr.FirstName, pr.Surname })
                    .FirstOrDefaultAsync(token) ??
                    throw new NotFoundException("Invalid Teacher");
                teacherInfo = (teacherData.FirstName, teacherData.Surname);
            }
            else
            {
                var teacherData = await _platformDb.UserInformations
                    .Where(pr => pr.AccountID == createSubjectDto.TeacherID)
                    .Select(pr => new { pr.FirstName, pr.Surname })
                    .FirstOrDefaultAsync(token) ??
                    throw new NotFoundException("Invalid Teacher");

                teacherInfo = (teacherData.FirstName, teacherData.Surname);
            }


            var subject = new Subject()
            {
                ClassID = classID,
                Name = createSubjectDto.SubjectName,
                Description = createSubjectDto.SubjectDescription,
                TeacherID = createSubjectDto.TeacherID ?? userID,
                TeacherName = teacherInfo.firstName,
                TeacherSurname = teacherInfo.surname,
            };
            await _platformDb.Subjects.AddAsync(subject, token);
            await _platformDb.SaveChangesAsync(token);

            return true;

        }

        public async Task<Subject> FindSubjectByTeacherID(string TeacherID, CancellationToken token)
        {
            return await _platformDb.Subjects.Where(pr => pr.TeacherID == TeacherID).FirstOrDefaultAsync(token)
                ?? throw new NotFoundException("Invalid Teacher ID");
        }

        public async Task<ELearningClass> FindClassById(string id, CancellationToken token)
        {
            if(Guid.TryParse(id, out var classID))
            {
                throw new BadRequestException("Invalid guid");
            }
            return await _platformDb.ELearningClasses.
                Where(pr=>pr.ELearningClassID==classID).FirstOrDefaultAsync(token)??
                throw new NotFoundException("Invalid class id");    
        }
    }
}
