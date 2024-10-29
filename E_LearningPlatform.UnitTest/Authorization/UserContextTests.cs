using ELearning_Platform.Infrastructure.Authorization;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using Xunit;

namespace E_LearningPlatform.UnitTest.Authorization
{
    public class UserContextTests
    {
        [Fact()]
        public void GetCurrentUserTest_ShouldBeOK()
        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, "1"),
                new(ClaimTypes.Email, "test@test.com"),
                new(ClaimTypes.Role, "student")
            };
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "test"));

            var httpContext = new Mock<IHttpContextAccessor>();

            httpContext
                .Setup(x => x.HttpContext)
                .Returns(new DefaultHttpContext()
                {
                    User = user,
                });

            var userContext = new UserContext(httpContext.Object);


            var currentUser = userContext.GetCurrentUser();

            currentUser.Should().NotBeNull();
            currentUser.UserID.Should().Be("1");
            currentUser.EmailAddress.Should().Be("test@test.com");
            currentUser.RoleName.Should().Be("student");

        }

        [Fact()]
        public void GetCurrentUserTest_ShouldBeInvalidClaims()
        {
            var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, "2"),
                new(ClaimTypes.Email, "test@test2.com"),
                new(ClaimTypes.Role, "student2")
            };
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "test"));

            var httpContext = new Mock<IHttpContextAccessor>();

            httpContext
                .Setup(x => x.HttpContext)
                .Returns(new DefaultHttpContext()
                {
                    User = user,
                });

            var userContext = new UserContext(httpContext.Object);


            var currentUser = userContext.GetCurrentUser();

            currentUser.Should().NotBeNull();
            currentUser.UserID.Should().NotBe("1");
            currentUser.EmailAddress.Should().NotBe("test@test.com");
            currentUser.RoleName.Should().NotBe("student");

        }
    }
}