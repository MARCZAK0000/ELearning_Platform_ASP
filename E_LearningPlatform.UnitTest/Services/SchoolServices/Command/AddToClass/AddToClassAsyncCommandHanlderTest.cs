using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Models.Notification;
using ELearning_Platform.Domain.Models.SchoolModel;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Infrastructure.Authorization;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
                ELearningClassID = Guid.NewGuid(),
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
                SubjectId = Guid.NewGuid(),
            } };

            var userContextMock = new Mock<IUserContext>();

            userContextMock.Setup(c => c.GetCurrentUser())
                .Returns(new CurrentUser("1", "test@test.com", "admin"));

            var schoolRepository = new Mock<ISchoolRepository>();

            schoolRepository.Setup(c => c.FindClassByClassIDAsync(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(elearningClass);

            schoolRepository.Setup(c => c.AddStudentToClassAsync(elearningClass, command, CancellationToken.None))
                .ReturnsAsync(new Domain.Response.ClassResponse.AddStudentToClassResponse
                {
                    IsSuccess = true,
                    AddedUsers = [],
                    ClassName = elearningClass.Name
                });

            schoolRepository.Setup(c=>c.FindSubjectByClassIDAsync(It.IsAny<Guid>(), CancellationToken.None)) 
                .ReturnsAsync(subject);


            schoolRepository.Setup(c => c.AddUsersToClassSubjectAsync(It.IsAny<List<Subject>>(), command.UsersToAdd,CancellationToken.None))
                .ReturnsAsync(new Domain.Response.ClassResponse.AddStudentToClassResponse
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
                (schoolRepository.Object, notificationMock.Object,userContextMock.Object);


            var result = await handler.Handle(command, CancellationToken.None);


            result.Result.Should().BeOfType<Ok>();
            schoolRepository.Verify(c => c.AddUsersToClassSubjectAsync(It.IsAny<List<Subject>>(), It.IsAny<IList<string>>(), CancellationToken.None), Times.Once);
        }
    }
}