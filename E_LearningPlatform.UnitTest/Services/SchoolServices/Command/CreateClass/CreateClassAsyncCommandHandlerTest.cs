using ELearning_Platform.Domain.Models.SchoolModel;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Response.ClassResponse;
using ELearning_Platform.Infrastructure.Authorization;
using ELearning_Platform.Infrastructure.Services.SchoolServices.Command.CreateClass;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Xunit;

namespace ELearning_Platform.Application.Services.SchoolServices.Command.CreateClass.Tests
{
    public class CreateClassAsyncCommandHandlerTest
    {
        [Fact()]
        public async Task CreateClassAsyncCommandHandlerTest_HandleTest_ShouldBeOK()
        {
            var command = new CreateClassAsyncCommand() 
            {
                Name = "Test",
                YearOfBegining = 2024,
                YearOfEnding = 2028
            };

            var schoolRepository = new Mock<ISchoolRepository>();

            schoolRepository
                .Setup(c => c.CreateClassAsync(It.IsAny<CreateClassDto>(), CancellationToken.None))
                .ReturnsAsync(new Domain.Response.ClassResponse.CreateClassResponse
                {
                    IsCreated = true,
                    Name = "Test",
                });

            var userContext = new Mock<IUserContext>();

            userContext.Setup(c => c.GetCurrentUser())
                .Returns(new CurrentUser("1", "test@test.com", "admin"));

            var handler = new CreateClassAsyncCommandHandler(schoolRepository.Object, userContext.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Result.Should().BeOfType<Ok<CreateClassResponse>>();

            schoolRepository.Verify(c => c.CreateClassAsync(It.IsAny<CreateClassDto>(), CancellationToken.None), Times.Once());
        }

        [Fact()]
        public async Task CreateClassAsyncCommandHandlerTest_HandleTest_ShouldBeError()
        {
            var command = new CreateClassAsyncCommand()
            {
                Name = "Test",
                YearOfBegining = 2024,
                YearOfEnding = 2028
            };

            var schoolRepository = new Mock<ISchoolRepository>();

            schoolRepository
                .Setup(c => c.CreateClassAsync(It.IsAny<CreateClassDto>(), CancellationToken.None))
                .ReturnsAsync(new Domain.Response.ClassResponse.CreateClassResponse
                {
                    IsCreated = false,
                    Name = "Test",
                });

            var userContext = new Mock<IUserContext>();

            userContext.Setup(c => c.GetCurrentUser())
                .Returns(new CurrentUser("1", "test@test.com", "admin"));

            var handler = new CreateClassAsyncCommandHandler(schoolRepository.Object, userContext.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Result.Should().BeOfType<ValidationProblem>()
                .Subject.ProblemDetails.Errors.Should()
                .BeEquivalentTo(new Dictionary<string, string[]>
                {
                    { "error", new[] { "Cannot Add Class" } }
                });

            schoolRepository.Verify(c => c.CreateClassAsync(It.IsAny<CreateClassDto>(), CancellationToken.None), Times.Once());
        }

        [Fact()]
        public async Task CreateClassAsyncCommandHandlerTest_HandleTest_Forbiden()
        {
            var command = new CreateClassAsyncCommand()
            {
                Name = "Test",
                YearOfBegining = 2024,
                YearOfEnding = 2028
            };

            var schoolRepository = new Mock<ISchoolRepository>();

            
            var userContext = new Mock<IUserContext>();

            userContext.Setup(c => c.GetCurrentUser())
                .Returns(new CurrentUser("1", "test@test.com", "student"));

            var handler = new CreateClassAsyncCommandHandler(schoolRepository.Object, userContext.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Result.Should().BeOfType<ForbidHttpResult>();

            schoolRepository.Verify(c => c.CreateClassAsync(It.IsAny<CreateClassDto>(), CancellationToken.None), Times.Never());
        }


    }
}