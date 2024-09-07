using Microsoft.AspNetCore.Identity;

namespace ELearning_Platform.Domain.Enitities
{
    public class Account : IdentityUser
    {
        public UserInformations User {  get; set; }

        public string? RefreshToken { get; set; }

        public DateOnly ModifidedDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }
}
