using ELearning_Platform.Application.Services.SchoolServices.Command.CreateLesson;
using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Models.Notification;
using ELearning_Platform.Domain.Models.SchoolModel;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Infrastructure.Authorization;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Xunit;


namespace E_LearningPlatform.UnitTest.Services.SchoolServices.Command.CreateLesson
{
    public class CreateLessonAsyncCommandHandlerTest
    {
        [Fact()]
        public async Task CreateLessonAsyncCommandHandlerTest_HandleTest_ShouldBeOKWithNotification()
        {
            var simpleClass = new ELearningClass()
            {
                ELearningClassID = Guid.NewGuid(),
                Name = "Test",
                ModifiedDate = DateTime.Now,
                YearOfBeggining = 2024,
                YearOfEnding = 2028,
                Students = [new UserInformations {
                    AccountID = "1",
                    FirstName = "John",
                    Surname = "Doe"
                }]
            };

            var subject = new Subject()
            {
                ClassID = simpleClass.ELearningClassID,
                Name = "TestSubject",
                SubjectId = Guid.NewGuid(),
                TeacherID = "1",
                ModifiedDate = DateTime.Now
            };

            var command = new CreateLessonAsyncCommand()
            {
                ClassID = simpleClass.ELearningClassID.ToString(),
                LessonDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                LessonName = "TestLesson",
                LessonDescription = "TestDescription",
                SubjectID = subject.SubjectId.ToString(),
            };

            var currentUserMock = new Mock<IUserContext>();
            var schoolRepositoryMock = new Mock<ISchoolRepository>();
            var lessonMaterialsMock = new Mock<ILessonMaterialsRepository>();
            var notificationRepository = new Mock<INotificaitonRepository>();

            currentUserMock.Setup(s => s.GetCurrentUser())
                .Returns(new CurrentUser("1", "test@test", "teacher"));

            schoolRepositoryMock
                .Setup(s => s.FindSubjectByIDAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(subject);

            schoolRepositoryMock
                .Setup(c =>
                    c.CreateLessonAsync
                        (It.IsAny<string>(), It.IsAny<Subject>(), It.IsAny<CreateLessonDto>(), CancellationToken.None))
                .ReturnsAsync(new Lesson
                {
                    LessonDate = command.LessonDate,
                    LessonID = Guid.NewGuid(),
                    LessonTopic = command.LessonName,
                    LessonDescription = command.LessonDescription,
                    TeacherID = "1",
                    SubjectID = subject.SubjectId,
                    ClassID = simpleClass.ELearningClassID,
                });


            schoolRepositoryMock.Setup(c
                => c.FindClassWithStudentsByIdAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(simpleClass);

            var handle = new
                CreateLessonAsyncCommandHandler(schoolRepositoryMock.Object,
                notificationRepository.Object, currentUserMock.Object, lessonMaterialsMock.Object);

            var result = await handle.Handle(command, CancellationToken.None);


            //Results 
            result.Result.Should().BeOfType<Ok<bool>>();

            schoolRepositoryMock
                .Verify(s => s.FindSubjectByIDAsync(It.IsAny<string>(), CancellationToken.None), Times.Once);

            currentUserMock.Verify(s => s.GetCurrentUser(), Times.Once);

            schoolRepositoryMock
                .Verify(c =>
                    c.CreateLessonAsync
                        (It.IsAny<string>(), It.IsAny<Subject>(), It.IsAny<CreateLessonDto>(), CancellationToken.None), Times.Once);

            lessonMaterialsMock.Verify(c
                => c.AddLessonMaterialsAsync(It.IsAny<List<IFormFile>>(), It.IsAny<Guid>(), CancellationToken.None), Times.Never);

            schoolRepositoryMock.Verify(c
               => c.FindClassWithStudentsByIdAsync(It.IsAny<string>(), CancellationToken.None), Times.Once);

            notificationRepository.Verify(c => c.CreateMoreThanOneNotificationAsync(It.IsAny<(string, string)>()
                , It.IsAny<List<CreateNotificationDto>>(), CancellationToken.None), Times.Once);


        }

        [Fact()]
        public async Task CreateLessonAsyncCommandHandlerTest_HandleTest_ShouldBeOKWithoutNotification()
        {
            var simpleClass = new ELearningClass()
            {
                ELearningClassID = Guid.NewGuid(),
                Name = "Test",
                ModifiedDate = DateTime.Now,
                YearOfBeggining = 2024,
                YearOfEnding = 2028,
            };

            var subject = new Subject()
            {
                ClassID = simpleClass.ELearningClassID,
                Name = "TestSubject",
                SubjectId = Guid.NewGuid(),
                TeacherID = "1",
                ModifiedDate = DateTime.Now
            };

            var command = new CreateLessonAsyncCommand()
            {
                ClassID = simpleClass.ELearningClassID.ToString(),
                LessonDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                LessonName = "TestLesson",
                LessonDescription = "TestDescription",
                SubjectID = subject.SubjectId.ToString(),
            };

            var currentUserMock = new Mock<IUserContext>();
            var schoolRepositoryMock = new Mock<ISchoolRepository>();
            var lessonMaterialsMock = new Mock<ILessonMaterialsRepository>();
            var notificationRepository = new Mock<INotificaitonRepository>();

            currentUserMock.Setup(s => s.GetCurrentUser())
                .Returns(new CurrentUser("1", "test@test", "teacher"));

            schoolRepositoryMock
                .Setup(s => s.FindSubjectByIDAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(subject);

            schoolRepositoryMock
                .Setup(c =>
                    c.CreateLessonAsync
                        (It.IsAny<string>(), It.IsAny<Subject>(), It.IsAny<CreateLessonDto>(), CancellationToken.None))
                .ReturnsAsync(new Lesson
                {
                    LessonDate = command.LessonDate,
                    LessonID = Guid.NewGuid(),
                    LessonTopic = command.LessonName,
                    LessonDescription = command.LessonDescription,
                    TeacherID = "1",
                    SubjectID = subject.SubjectId,
                    ClassID = simpleClass.ELearningClassID,
                });


            schoolRepositoryMock.Setup(c
                => c.FindClassWithStudentsByIdAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(simpleClass);

            var handle = new
                CreateLessonAsyncCommandHandler(schoolRepositoryMock.Object,
                notificationRepository.Object, currentUserMock.Object, lessonMaterialsMock.Object);

            var result = await handle.Handle(command, CancellationToken.None);


            //Results 
            result.Result.Should().BeOfType<Ok<bool>>();

            schoolRepositoryMock
                .Verify(s => s.FindSubjectByIDAsync(It.IsAny<string>(), CancellationToken.None), Times.Once);

            currentUserMock.Verify(s => s.GetCurrentUser(), Times.Once);

            schoolRepositoryMock
                .Verify(c =>
                    c.CreateLessonAsync
                        (It.IsAny<string>(), It.IsAny<Subject>(), It.IsAny<CreateLessonDto>(), CancellationToken.None), Times.Once);
            lessonMaterialsMock.Verify(c
                => c.AddLessonMaterialsAsync(It.IsAny<List<IFormFile>>(), It.IsAny<Guid>(), CancellationToken.None), Times.Never);

            schoolRepositoryMock.Verify(c
               => c.FindClassWithStudentsByIdAsync(It.IsAny<string>(), CancellationToken.None), Times.Once);

            notificationRepository.Verify(c => c.CreateMoreThanOneNotificationAsync(It.IsAny<(string, string)>()
                , It.IsAny<List<CreateNotificationDto>>(), CancellationToken.None), Times.Never);


        }

        [Fact()]
        public async Task CreateLessonAsyncCommandHandlerTest_HandleTest_ShouldBeForbid()
        {
            var simpleClass = new ELearningClass()
            {
                ELearningClassID = Guid.NewGuid(),
                Name = "Test",
                ModifiedDate = DateTime.Now,
                YearOfBeggining = 2024,
                YearOfEnding = 2028,
            };

            var subject = new Subject()
            {
                ClassID = simpleClass.ELearningClassID,
                Name = "TestSubject",
                SubjectId = Guid.NewGuid(),
                TeacherID = "2",
                ModifiedDate = DateTime.Now
            };

            var command = new CreateLessonAsyncCommand()
            {
                ClassID = simpleClass.ELearningClassID.ToString(),
                LessonDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                LessonName = "TestLesson",
                LessonDescription = "TestDescription",
                SubjectID = subject.SubjectId.ToString(),
            };

            var currentUserMock = new Mock<IUserContext>();
            var schoolRepositoryMock = new Mock<ISchoolRepository>();
            var lessonMaterialsMock = new Mock<ILessonMaterialsRepository>();
            var notificationRepository = new Mock<INotificaitonRepository>();

            currentUserMock.Setup(s => s.GetCurrentUser())
             .Returns(new CurrentUser("1", "test@test", "teacher"));


            var handle = new
               CreateLessonAsyncCommandHandler(schoolRepositoryMock.Object,
               notificationRepository.Object, currentUserMock.Object, lessonMaterialsMock.Object);

            var result = await handle.Handle(command, CancellationToken.None);

            result.Result.Should().BeOfType<ForbidHttpResult>();

            schoolRepositoryMock
                .Verify(s => s.FindSubjectByIDAsync(It.IsAny<string>(), CancellationToken.None), Times.Once);

            currentUserMock.Verify(s => s.GetCurrentUser(), Times.Once);

            schoolRepositoryMock
                .Verify(c =>
                    c.CreateLessonAsync
                        (It.IsAny<string>(), It.IsAny<Subject>(), It.IsAny<CreateLessonDto>(), CancellationToken.None), Times.Never);

            lessonMaterialsMock.Verify(c
                => c.AddLessonMaterialsAsync(It.IsAny<List<IFormFile>>(), It.IsAny<Guid>(), CancellationToken.None), Times.Never);

            schoolRepositoryMock.Verify(c
               => c.FindClassWithStudentsByIdAsync(It.IsAny<string>(), CancellationToken.None), Times.Never);

            notificationRepository.Verify(c => c.CreateMoreThanOneNotificationAsync(It.IsAny<(string, string)>()
                , It.IsAny<List<CreateNotificationDto>>(), CancellationToken.None), Times.Never);


        }

        [Fact()]
        public async Task CreateLessonAsyncCommandHandlerTest_HandleTest_ShouldBeValidationProblems()
        {
            var simpleClass = new ELearningClass()
            {
                ELearningClassID = Guid.NewGuid(),
                Name = "Test",
                ModifiedDate = DateTime.Now,
                YearOfBeggining = 2024,
                YearOfEnding = 2028,
            };

            var subject = new Subject()
            {
                ClassID = simpleClass.ELearningClassID,
                Name = "TestSubject",
                SubjectId = Guid.NewGuid(),
                TeacherID = "1",
                ModifiedDate = DateTime.Now
            };

            var command = new CreateLessonAsyncCommand()
            {
                ClassID = simpleClass.ELearningClassID.ToString(),
                LessonDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                LessonName = "TestLesson",
                LessonDescription = "TestDescription",
                SubjectID = subject.SubjectId.ToString(),
            };

            var currentUserMock = new Mock<IUserContext>();
            var schoolRepositoryMock = new Mock<ISchoolRepository>();
            var lessonMaterialsMock = new Mock<ILessonMaterialsRepository>();
            var notificationRepository = new Mock<INotificaitonRepository>();

            currentUserMock.Setup(s => s.GetCurrentUser())
                .Returns(new CurrentUser("1", "test@test", "teacher"));

            schoolRepositoryMock
                .Setup(s => s.FindSubjectByIDAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(subject);

            schoolRepositoryMock
                .Setup(c =>
                    c.CreateLessonAsync
                        (It.IsAny<string>(), It.IsAny<Subject>(), It.IsAny<CreateLessonDto>(), CancellationToken.None))
                .ReturnsAsync((Lesson)null!);

            var handle = new
                CreateLessonAsyncCommandHandler(schoolRepositoryMock.Object,
                notificationRepository.Object, currentUserMock.Object, lessonMaterialsMock.Object);

            var result = await handle.Handle(command, CancellationToken.None);


            //Results 
            result.Result.Should().BeOfType<ValidationProblem>()
              .Subject.ProblemDetails.Errors.Should()
              .BeEquivalentTo(new Dictionary<string, string[]>
              {
                    { "error", new[] { "Cannot create Lesson" } }
              });

            schoolRepositoryMock
                .Verify(s => s.FindSubjectByIDAsync(It.IsAny<string>(), CancellationToken.None), Times.Once);

            currentUserMock.Verify(s => s.GetCurrentUser(), Times.Once);

            schoolRepositoryMock
                .Verify(c =>
                    c.CreateLessonAsync
                        (It.IsAny<string>(), It.IsAny<Subject>(), It.IsAny<CreateLessonDto>(), CancellationToken.None), Times.Once);

            lessonMaterialsMock.Verify(c
                => c.AddLessonMaterialsAsync(It.IsAny<List<IFormFile>>(), It.IsAny<Guid>(), CancellationToken.None), Times.Never);

            schoolRepositoryMock.Verify(c
               => c.FindClassWithStudentsByIdAsync(It.IsAny<string>(), CancellationToken.None), Times.Never);

            notificationRepository.Verify(c => c.CreateMoreThanOneNotificationAsync(It.IsAny<(string, string)>()
                , It.IsAny<List<CreateNotificationDto>>(), CancellationToken.None), Times.Never);


        }
    }
}