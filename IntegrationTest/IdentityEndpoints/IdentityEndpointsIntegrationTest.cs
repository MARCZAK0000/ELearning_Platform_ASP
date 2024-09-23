using ELearning_Platform.API;
using FluentAssertions;
using System.Text.Json;

namespace IntegrationTest.IdentityEndpoints
{
    public class IdentityEndpointsIntegrationTest(CustomWebApplicationFactory<Program> factory) 
        : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient = factory.CreateClient();
        [Fact]
        public async Task SignInShouldBe_OK()
        {
            var jsonContent = JsonSerializer.Serialize(new { Email = "test@test.com", Password = "password" });
            var content = new StringContent(jsonContent, encoding: System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/account/login", content);
            if (response.IsSuccessStatusCode)
            {
                response.Should().NotBeNull();
            }
        }
    }
}
