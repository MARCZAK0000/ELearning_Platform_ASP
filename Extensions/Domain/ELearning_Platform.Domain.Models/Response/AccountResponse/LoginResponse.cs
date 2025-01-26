using ELearning_Platform.Domain.Models.Models.AccountModel;

namespace ELearning_Platform.Domain.Models.Response.AccountResponse
{
    public class LoginResponse
    {
        public bool Success { get; set; }

        public string? Email { get; set; }

        public string? Role { get; set; }

        public TokenModelDto? TokenModelDto { get; set; }
    }
}
