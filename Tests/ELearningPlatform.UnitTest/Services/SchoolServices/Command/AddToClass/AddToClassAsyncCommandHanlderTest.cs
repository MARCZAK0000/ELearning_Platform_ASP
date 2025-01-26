using ELearning_Platform.Application.Authorization.Authorization;
using ELearning_Platform.Application.MediatR.Services.SchoolServices.Command.AddToClass;
using ELearning_Platform.Domain.Core.Repository;
using ELearning_Platform.Domain.Database.Enitities;
using ELearning_Platform.Domain.Models.Models.Notification;
using ELearning_Platform.Domain.Models.Models.SchoolModel;
using ELearning_Platform.Domain.Models.Response.ClassResponse;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using Xunit;

namespace ELearning_Platform.Infrastructure.Services.SchoolServices.Command.AddToClass.Tests
{
    public class AddToClassAsyncCommandHanlderTest
    {
        [Fact()]
        public async Task AddToClassAsyncCommandHanlder_HandleTest_ShouldBeOK()
        {
            var elearningClass = new ELearningClass()
            {
                ELearningClassID = Guid.NewGuid().ToString(),
                Name = "Test",
                YearOfBeggining = 2025,
                YearOfEnding = 2029,
                ModifiedDate = DateTime.Now,
            };

            var command = new AddToClassAsyncCommand()
            {
                ClassID = elearningClass.ELearningClassID,
                UsersToAdd = ["1", "2"]
            };


            var subject = new List<Subject>() { new()
            {
                ClassID = elearningClass.ELearningClassID,
                Name = "Test",
                Description = "TestTest",
                TeacherID = "1",
            } };

            var userContextMock = new Mock<IUserContext>();

            userContextMock.Setup(c => c.GetCurrentUser())
                .Returns(new CurrentUser("1", "test@test.com", "admin"));

            var schoolRepository = new Mock<ISchoolRepository>();

            schoolRepository.Setup(c => c.FindClassByClassIDAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(elearningClass);

            schoolRepository.Setup(c => c.AddStudentToClassAsync(elearningClass, command, CancellationToken.None))
                .ReturnsAsync(new AddStudentToClassResponse
                {
                    IsSuccess = true,
                    AddedUsers = [],
                    ClassName = elearningClass.Name
                });

            schoolRepository.Setup(c => c.FindSubjectByClassIDAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(subject);


            schoolRepository.Setup(c => c.AddUsersToClassSubjectAsync(It.IsAny<List<Subject>>(), command.UsersToAdd, CancellationToken.None))
                .ReturnsAsync(new AddStudentToClassResponse
                {
                    IsSuccess = true
                });

            var notificationMock = new Mock<INotificaitonRepository>();

            (string email, string userName) currentUser = ("test@test.com", "test");

            notificationMock.Setup(pr => pr.CreateMoreThanOneNotificationAsync(
                currentUser,
                It.IsAny<List<CreateNotificationDto>>(),
                CancellationToken.None));

            var handler = new AddToClassAsyncCommandHanlder
                (schoolRepository.Object, notificationMock.Object, userContextMock.Object);


            var result = await handler.Handle(command, CancellationToken.None);


            result.Result.Should().BeOfType<Ok>();
            schoolRepository.Verify(c => c.AddUsersToClassSubjectAsync(It.IsAny<List<Subject>>(), It.IsAny<IList<string>>(), CancellationToken.None), Times.Once);
        }

        [Fact()]
        public async Task AddToClassAsyncCommandHanlder_HandleTest_ValidationProblems_CannotAddToSubjects()
        {
            var elearningClass = new ELearningClass()
            {
                Name = "Test",
                YearOfBeggining = 2025,
                YearOfEnding = 2029,
                ModifiedDate = DateTime.Now,
            };

            var command = new AddToClassAsyncCommand()
            {
                ClassID = elearningClass.ELearningClassID,
                UsersToAdd = ["1", "2"]
            };


            var subject = new List<Subject>() { new()
            {
                ClassID = elearningClass.ELearningClassID,
                Name = "Test",
                Description = "TestTest",
                TeacherID = "1",
            } };

            var userContextMock = new Mock<IUserContext>();

            userContextMock.Setup(c => c.GetCurrentUser())
                .Returns(new CurrentUser("1", "test@test.com", "admin"));

            var schoolRepository = new Mock<ISchoolRepository>();

            schoolRepository.Setup(c => c.FindClassByClassIDAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(elearningClass);

            schoolRepository.Setup(c => c.AddStudentToClassAsync(elearningClass, command, CancellationToken.None))
                .ReturnsAsync(new AddStudentToClassResponse
                {
                    IsSuccess = true,
                    AddedUsers = [],
                    ClassName = elearningClass.Name
                });

            schoolRepository.Setup(c => c.FindSubjectByClassIDAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(subject);


            schoolRepository.Setup(c => c.AddUsersToClassSubjectAsync(It.IsAny<List<Subject>>(), command.UsersToAdd, CancellationToken.None))
                .ReturnsAsync(new AddStudentToClassResponse
                {
                    IsSuccess = false
                });

            var notificationMock = new Mock<INotificaitonRepository>();

            var handler = new AddToClassAsyncCommandHanlder
                (schoolRepository.Object, notificationMock.Object, userContextMock.Object);


            var result = await handler.Handle(command, CancellationToken.None);


            result.Result.Should().BeOfType<ValidationProblem>()
                .Subject.ProblemDetails.Errors.Should()
                .BeEquivalentTo(new Dictionary<string, string[]>
                {
                    { "error", new[] { "problem with database" } }
                });
            schoolRepository.Verify(c => c.AddUsersToClassSubjectAsync(It.IsAny<List<Subject>>(), It.IsAny<IList<string>>(), CancellationToken.None), Times.Once);

            notificationMock
                .Verify(c => c.CreateMoreThanOneNotificationAsync
                    (It.IsAny<(string, string)>(), It.IsAny<List<CreateNotificationDto>>(), CancellationToken.None), Times.Never);
        }

        [Fact()]
        public async Task AddToClassAsyncCommandHanlder_HandleTest_NotFoundSubject_ResultStillOK()
        {
            var elearningClass = new ELearningClass()
            {
                Name = "Test",
                YearOfBeggining = 2025,
                YearOfEnding = 2029,
                ModifiedDate = DateTime.Now,
            };

            var command = new AddToClassAsyncCommand()
            {
                ClassID = elearningClass.ELearningClassID,
                UsersToAdd = ["1", "2"]
            };


            var userContextMock = new Mock<IUserContext>();

            userContextMock.Setup(c => c.GetCurrentUser())
                .Returns(new CurrentUser("1", "test@test.com", "admin"));

            var schoolRepository = new Mock<ISchoolRepository>();

            schoolRepository.Setup(c => c.FindClassByClassIDAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(elearningClass);

            schoolRepository.Setup(c => c.AddStudentToClassAsync(elearningClass, command, CancellationToken.None))
                .ReturnsAsync(new AddStudentToClassResponse
                {
                    IsSuccess = true,
                    AddedUsers = [],
                    ClassName = elearningClass.Name
                });

            schoolRepository.Setup(c => c.FindSubjectByClassIDAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync((List<Subject>)null!);

            var notificationMock = new Mock<INotificaitonRepository>();

            var handler = new AddToClassAsyncCommandHanlder
                (schoolRepository.Object, notificationMock.Object, userContextMock.Object);


            var result = await handler.Handle(command, CancellationToken.None);

            result.Result.Should().BeOfType<Ok>();

            schoolRepository
                .Verify(c => c.AddUsersToClassSubjectAsync
                    (It.IsAny<List<Subject>>(), It.IsAny<IList<string>>(), CancellationToken.None), Times.Never);

            notificationMock
                .Verify(c => c.CreateMoreThanOneNotificationAsync
                    (It.IsAny<(string, string)>(), It.IsAny<List<CreateNotificationDto>>(), CancellationToken.None), Times.Once);
        }

        [Fact()]
        public async Task AddToClassAsyncCommandHanlder_HandleTest_ShouldBeValidationProblems_CannotAddPerson()
        {
            var elearningClass = new ELearningClass()
            {
                Name = "Test",
                YearOfBeggining = 2025,
                YearOfEnding = 2029,
                ModifiedDate = DateTime.Now,
            };

            var command = new AddToClassAsyncCommand()
            {
                ClassID = elearningClass.ELearningClassID,
                UsersToAdd = ["1", "2"]
            };

            var userContextMock = new Mock<IUserContext>();

            userContextMock.Setup(c => c.GetCurrentUser())
                .Returns(new CurrentUser("1", "test@test.com", "admin"));

            var schoolRepository = new Mock<ISchoolRepository>();

            schoolRepository.Setup(c => c.FindClassByClassIDAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync(elearningClass);

            schoolRepository.Setup(c => c.AddStudentToClassAsync(elearningClass, command, CancellationToken.None))
                .ReturnsAsync(new AddStudentToClassResponse
                {
                    IsSuccess = false,
                    AddedUsers = [],
                    ClassName = elearningClass.Name
                });

            var notificationMock = new Mock<INotificaitonRepository>();

            var handler = new AddToClassAsyncCommandHanlder
                (schoolRepository.Object, notificationMock.Object, userContextMock.Object);


            var result = await handler.Handle(command, CancellationToken.None);

            result.Result.Should().BeOfType<ValidationProblem>()
                .Subject.ProblemDetails.Errors.Should()
                .BeEquivalentTo(new Dictionary<string, string[]>
                {
                    { "error", new[] { "Cannot add students to database" } }
                });


            schoolRepository
               .Verify(c => c.AddStudentToClassAsync(It.IsAny<ELearningClass>(), It.IsAny<AddStudentToClassDto>(), CancellationToken.None), Times.Once());

            schoolRepository
                .Verify(c => c.AddUsersToClassSubjectAsync
                    (It.IsAny<List<Subject>>(), It.IsAny<IList<string>>(), CancellationToken.None), Times.Never);

            schoolRepository
               .Verify(c => c.FindSubjectByClassIDAsync(It.IsAny<string>(), CancellationToken.None), Times.Never());

            notificationMock
               .Verify(c => c.CreateMoreThanOneNotificationAsync
                   (It.IsAny<(string, string)>(), It.IsAny<List<CreateNotificationDto>>(), CancellationToken.None), Times.Never);
        }

        [Fact()]
        public async Task AddToClassAsyncCommandHanlder_HandleTest_NotFound_CannotFindClass()
        {

            var command = new AddToClassAsyncCommand()
            {
                UsersToAdd = ["1", "2"]
            };


            var userContextMock = new Mock<IUserContext>();

            userContextMock.Setup(c => c.GetCurrentUser())
                .Returns(new CurrentUser("1", "test@test.com", "admin"));

            var schoolRepository = new Mock<ISchoolRepository>();

            schoolRepository.Setup(c => c.FindClassByClassIDAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync((ELearningClass)null!);

            var notificationMock = new Mock<INotificaitonRepository>();

            var handler = new AddToClassAsyncCommandHanlder
                (schoolRepository.Object, notificationMock.Object, userContextMock.Object);


            var result = await handler.Handle(command, CancellationToken.None);


            result.Result.Should().BeOfType<NotFound<ProblemDetails>>()
                .Subject.Should().BeEquivalentTo(new ProblemDetails
                {
                    Title = "Cannot Found Class By Class ID",
                    Status = (int)HttpStatusCode.NotFound,
                }, options => options.ExcludingMissingMembers());

            schoolRepository
                .Verify(c => c.FindClassByClassIDAsync(It.IsAny<string>(), CancellationToken.None), Times.Once());
            schoolRepository
                .Verify(c => c.AddStudentToClassAsync(It.IsAny<ELearningClass>(), It.IsAny<AddStudentToClassDto>(), CancellationToken.None), Times.Never());
            schoolRepository
                .Verify(c => c.FindSubjectByClassIDAsync(It.IsAny<string>(), CancellationToken.None), Times.Never());
            schoolRepository
                .Verify(c => c.AddUsersToClassSubjectAsync(It.IsAny<List<Subject>>(), It.IsAny<IList<string>>(), CancellationToken.None), Times.Never);
            notificationMock
               .Verify(c => c.CreateMoreThanOneNotificationAsync
                   (It.IsAny<(string, string)>(), It.IsAny<List<CreateNotificationDto>>(), CancellationToken.None), Times.Never);
        }

        [Fact()]
        public async Task AddToClassAsyncCommandHanlder_HandleTest_Forbiden()
        {

            var command = new AddToClassAsyncCommand()
            {
                UsersToAdd = ["1", "2"]
            };

            var userContextMock = new Mock<IUserContext>();

            userContextMock.Setup(c => c.GetCurrentUser())
                .Returns(new CurrentUser("1", "test@test.com", "student"));

            var schoolRepository = new Mock<ISchoolRepository>();


            var notificationMock = new Mock<INotificaitonRepository>();


            var handler = new AddToClassAsyncCommandHanlder
                (schoolRepository.Object, notificationMock.Object, userContextMock.Object);


            var result = await handler.Handle(command, CancellationToken.None);

            result.Result.Should().BeOfType<ForbidHttpResult>();

            schoolRepository
                .Verify(c => c.FindClassByClassIDAsync(It.IsAny<string>(), CancellationToken.None), Times.Never());
            schoolRepository
                .Verify(c => c.AddStudentToClassAsync(It.IsAny<ELearningClass>(), It.IsAny<AddStudentToClassDto>(), CancellationToken.None), Times.Never());
            schoolRepository
                .Verify(c => c.FindSubjectByClassIDAsync(It.IsAny<string>(), CancellationToken.None), Times.Never());
            schoolRepository
                .Verify(c => c.AddUsersToClassSubjectAsync(It.IsAny<List<Subject>>(), It.IsAny<IList<string>>(), CancellationToken.None), Times.Never);
            notificationMock
               .Verify(c => c.CreateMoreThanOneNotificationAsync
                   (It.IsAny<(string, string)>(), It.IsAny<List<CreateNotificationDto>>(), CancellationToken.None), Times.Never);
        }
    }
}