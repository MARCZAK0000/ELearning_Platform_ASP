using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Models.AccountModel;
using Microsoft.AspNetCore.Identity;

namespace ELearning_Platform.Infrastructure.Database
{
    public class TestSeederDb(UserManager<Account> userManager, PlatformDb platformDb)
    {
        private readonly UserManager<Account> _userManager = userManager;
        private readonly PlatformDb _platformDb = platformDb;
        public async Task AddTestUserAsync()
        {
            var testUser = new Account()
            {
                Email = "test@test.com",
                UserName = "test@test.com",
                PhoneNumber = "111222333",
                User = new UserInformations()
                {
                    FirstName = "Joe",
                    Surname = "Doe",
                    PhoneNumber = "111222333",
                    EmailAddress = "test@test.com",
                    Address = new UserAddress()
                    {
                        City = "Warsaw",
                        Country = "Poland",
                        StreetName = "Wiejska",
                        PostalCode = "00-000",
                    }
                }
            };
            testUser.PasswordHash = _userManager.PasswordHasher.HashPassword(user: testUser, password: "password");
            testUser.User.AccountID = testUser.Id;
            testUser.User.Address.AccountID = testUser.Id;
            if (await _userManager.FindByEmailAsync(testUser.Email) != null)
            {
                await _userManager.CreateAsync(testUser);
                await _platformDb.SaveChangesAsync();           
            }
        }
    }
}
