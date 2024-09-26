using ELearning_Platform.Domain.Models.AccountModel;
using Microsoft.AspNetCore.Identity;

namespace ELearning_Platform.Domain.Response.AccountResponse
{
    public class LoginResponse
    {
        public SignInResult Success { get; set; }

        public TokenModelDto? TokenModelDto { get; set; }
    }
}
