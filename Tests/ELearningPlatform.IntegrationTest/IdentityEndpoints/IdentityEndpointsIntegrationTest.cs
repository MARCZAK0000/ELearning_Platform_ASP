using E_LearningPlatform.IntegrationTest.Data;
using ELearning_Platform.API;
using ELearning_Platform.Domain.Models.Models.AccountModel;
using FluentAssertions;
using IntegrationTest;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Text.Json;

namespace E_LearningPlatform.IntegrationTest.IdentityEndpoints
{
    public class IdentityEndpointsIntegrationTest(CustomWebApplicationFactory<Program> factory)
        : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient = factory.CreateClient();
        [Theory]
        [ClassData(typeof(RegisterUserTheory))]
        public async Task RegisterShouldBe_OK(string email, string password, string phone)
        {
            var jsonContent = JsonSerializer.Serialize(new RegisterModelDto()
            {
                AddressEmail = email,
                Password = password,
                ConfirmPassword = password,
                FirstName = "John",
                SecondName = "John",
                Surname = "Doe",
                PhoneNumber = phone,
                City = "New York",
                Country = "USA",
                StreetName = "Brooklyn",
                PostalCode = "00-000",
            });
            var content = new StringContent(jsonContent, encoding: System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/account/registration", content);

            response.Should().NotBeNull();
            response.Should().HaveStatusCode(System.Net.HttpStatusCode.Created);
        }
        [Fact]
        public async Task RegisterShouldBe_Fail()
        {
            var jsonContent = JsonSerializer.Serialize(new RegisterModelDto()
            {
                AddressEmail = "test@test.com",
                Password = "password",
                ConfirmPassword = "password",
                FirstName = "John",
                SecondName = "John",
                Surname = "Doe",
                PhoneNumber = "111222333",
                City = "New York",
                Country = "USA",
                StreetName = "Brooklyn",
                PostalCode = "00-000",
            });
            var content = new StringContent(jsonContent, encoding: System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/account/registration", content);

            response.Should().HaveStatusCode(System.Net.HttpStatusCode.BadRequest);
            var message = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            message.Should().NotBeNull();
            message!.Title.Should().Be("Invalid Credentials");
            message!.Detail.Should().Be("Email is in used");
        }
        [Theory]
        [ClassData(typeof(SignInUsersTheory))]
        public async Task SignInShouldBe_OK(string email, string password)
        {
            var jsonContent = JsonSerializer.Serialize(new { EmailAddress = email, Password = password });
            var content = new StringContent(jsonContent, encoding: System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/account/signin", content);

            response.Should().NotBeNull();
            response.IsSuccessStatusCode.Should().BeTrue();
            var message = await response.Content.ReadFromJsonAsync<bool>();
            message.Should().BeTrue();

        }

        [Fact]
        public async Task SignInShouldBe_UnAuthorized()
        {
            var jsonContent = JsonSerializer.Serialize(new { EmailAddress = "test2@test.com", Password = "password" });
            var content = new StringContent(jsonContent, encoding: System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/account/signin", content);



            response.Should().HaveStatusCode(System.Net.HttpStatusCode.NotFound);


        }
    }
}
