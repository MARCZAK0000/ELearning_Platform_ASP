using ELearning_Platform.Domain.Models.AccountModel;
using Microsoft.AspNetCore.Identity;

namespace ELearning_Platform.Domain.Response.AccountResponse
{
    public class LoginResponse
    {
        public bool Success { get; set; }

        public string? Email { get; set; }

        public string? Role { get; set; } 

        public TokenModelDto? TokenModelDto { get; set; }
    }
}
