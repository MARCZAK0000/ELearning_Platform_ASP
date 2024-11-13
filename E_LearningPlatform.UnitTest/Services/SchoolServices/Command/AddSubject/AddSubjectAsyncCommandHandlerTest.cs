﻿using ELearning_Platform.Application.Services.SchoolServices.Command.AddSubject;
using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Exceptions;
using ELearning_Platform.Domain.Models.SchoolModel;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Infrastructure.Authorization;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Xunit;

namespace E_LearningPlatform.UnitTest.Services.SchoolServices.Command.AddSubject
{
    public class AddSubjectAsyncCommandHandlerTest
    {
        [Fact()]
        public async void AddSubjectAsyncCommandHandler_HandleTest_ShouldBeTrue()
        {
            var elearningClass = new ELearningClass()
            {
                ELearningClassID = Guid.NewGuid(),
                Name = "Test",
                YearOfBeggining = 2025,
                YearOfEnding = 2029,
                ModifiedDate = DateTime.Now,
            };


            var command = new AddSubjectAsyncCommand()
            {
                ClassID = elearningClass.ELearningClassID.ToString(),
                SubjectName = "Test",
                SubjectDescription = "TestTestTestTest",
                TeacherID = "1"
            };

            var userContextMock = new Mock<IUserContext>();

            userContextMock.Setup(c => c.GetCurrentUser())
                .Returns(new CurrentUser("1", "test@test.com", "teacher"));

            var schoolRepo = new Mock<ISchoolRepository>();

            schoolRepo
                .Setup(c => c.FindClassWithStudentsByIdAsync(elearningClass.ELearningClassID.ToString(), CancellationToken.None))
                .ReturnsAsync(elearningClass);
            

            var handler = new AddSubjectAsyncCommandHandler(schoolRepo.Object, userContextMock.Object);

            var result = await handler.Handle(command, CancellationToken.None);


            //assert 

            result.Should().BeTrue();  

            schoolRepo.Verify(c=>c.CreateSubjectAsync(It.IsAny<string>(), It.IsAny<CreateSubjectDto>(), CancellationToken.None), Times.Once);
        }


        [Fact()]
        public async void AddSubjectAsyncCommandHandler_HandleTest_ShouldBeInvalidClassError()
        {
            var elearningClass = new ELearningClass()
            {
                ELearningClassID = Guid.NewGuid(),
                Name = "Test",
                YearOfBeggining = 2025,
                YearOfEnding = 2029,
                ModifiedDate = DateTime.Now,
            };


            var command = new AddSubjectAsyncCommand()
            {
                ClassID = "11111",
                SubjectName = "Test",
                SubjectDescription = "TestTestTestTest",
                TeacherID = "1"
            };

            var userContextMock = new Mock<IUserContext>();

            userContextMock.Setup(c => c.GetCurrentUser())
                .Returns(new CurrentUser("1", "test@test.com", "teacher"));

            var schoolRepo = new Mock<ISchoolRepository>();

            schoolRepo
                .Setup(c => c.FindClassWithStudentsByIdAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync((ELearningClass)null!);

            var handler = new AddSubjectAsyncCommandHandler(schoolRepo.Object, userContextMock.Object);

            Func<Task> act = async()=> await handler.Handle(command, CancellationToken.None);

            //assert 
            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("Invalid ClassID");
            schoolRepo.Verify(c => c.CreateSubjectAsync(It.IsAny<string>(), It.IsAny<CreateSubjectDto>(), CancellationToken.None), Times.Never);
        }


        [Fact()]
        public async void AddSubjectAsyncCommandHandler_HandleTest_ShouldBeForbbidenError()
        {
            var elearningClass = new ELearningClass()
            {
                ELearningClassID = Guid.NewGuid(),
                Name = "Test",
                YearOfBeggining = 2025,
                YearOfEnding = 2029,
                ModifiedDate = DateTime.Now,
            };


            var command = new AddSubjectAsyncCommand()
            {
                ClassID = elearningClass.ELearningClassID.ToString(),
                SubjectName = "Test",
                SubjectDescription = "TestTestTestTest",
                TeacherID = "1"
            };

            var userContextMock = new Mock<IUserContext>();

            userContextMock.Setup(c => c.GetCurrentUser())
                .Returns(new CurrentUser("1", "test@test.com", "student"));

            var schoolRepo = new Mock<ISchoolRepository>();

            schoolRepo
                .Setup(c => c.FindClassWithStudentsByIdAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync((ELearningClass)null!);

            var handler = new AddSubjectAsyncCommandHandler(schoolRepo.Object, userContextMock.Object);

            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            //assert 
            await act.Should().ThrowAsync<ForbidenException>()
                .WithMessage("Forbiden action");
            schoolRepo.Verify(c => c.CreateSubjectAsync(It.IsAny<string>(), It.IsAny<CreateSubjectDto>(), CancellationToken.None), Times.Never);
        }

        [Fact()]
        public async void AddSubjectAsyncCommandHandler_HandleTest_ShouldBeInvalidTeacherID()
        {
            var elearningClass = new ELearningClass()
            {
                ELearningClassID = Guid.NewGuid(),
                Name = "Test",
                YearOfBeggining = 2025,
                YearOfEnding = 2029,
                ModifiedDate = DateTime.Now,
            };


            var command = new AddSubjectAsyncCommand()
            {
                ClassID = elearningClass.ELearningClassID.ToString(),
                SubjectName = "Test",
                SubjectDescription = "TestTestTestTest",
                TeacherID = null
            };

            var userContextMock = new Mock<IUserContext>();

            userContextMock.Setup(c => c.GetCurrentUser())
                .Returns(new CurrentUser("1", "test@test.com", "moderator"));

            var schoolRepo = new Mock<ISchoolRepository>();

            schoolRepo
                .Setup(c => c.FindClassWithStudentsByIdAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync((ELearningClass)null!);

            var handler = new AddSubjectAsyncCommandHandler(schoolRepo.Object, userContextMock.Object);

            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            //assert 
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Invalid TeacherID");
            schoolRepo.Verify(c => c.CreateSubjectAsync(It.IsAny<string>(), It.IsAny<CreateSubjectDto>(), CancellationToken.None), Times.Never);
        }
    }
}