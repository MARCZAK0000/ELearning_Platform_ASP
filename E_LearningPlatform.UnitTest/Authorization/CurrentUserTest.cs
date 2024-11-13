﻿using ELearning_Platform.Infrastructure.Authorization;
using FluentAssertions;
using System.Security.Claims;
using Xunit;

namespace E_LearningPlatform.UnitTest.Authorization
{
    public class CurrentUserTest
    {
        [Fact()]
        public void IsInRoleTest_ShouldBeOK()
        {
           
            var currentUser = new CurrentUser("1", "test@test.com", "student");

            var result = currentUser.IsInRole("student");

            result.Should().BeTrue();

        }

        [Fact()]
        public void IsInRoleTest_ShouldBeFailed()
        {

            var currentUser = new CurrentUser("1", "test@test.com", "student");

            var result = currentUser.IsInRole("admin");

            result.Should().BeFalse();

        }
    }
}