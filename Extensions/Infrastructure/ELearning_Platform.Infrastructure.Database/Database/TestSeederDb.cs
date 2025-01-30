using ELearning_Platform.Domain.Database.Enitities;
using Microsoft.AspNetCore.Identity;

namespace ELearning_Platform.Infrastructure.Database.Database
{
    public class TestSeederDb(UserManager<Account> userManager, PlatformDb platformDb)
    {
        private readonly UserManager<Account> _userManager = userManager;
        private readonly PlatformDb _platformDb = platformDb;
        public async Task AddTestUserAsync()
        {
            var testUsers = new List<Account>()
            {
                new()
                {
                    Email = "test@test.com",
                    UserName = "test@test.com",
                    PhoneNumber = "111222333",
                    User = new User()
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
                },
                new()
                {
                    Email = "test1@test.com",
                    UserName = "test1@test.com",
                    PhoneNumber = "222222333",
                    User = new User()
                    {
                        FirstName = "Joe",
                        Surname = "Doe",
                        PhoneNumber = "222222333",
                        EmailAddress = "test1@test.com",
                        Address = new UserAddress()
                        {
                            City = "Warsaw",
                            Country = "Poland",
                            StreetName = "Wiejska",
                            PostalCode = "00-000",
                        }
                    }
                }
            };
            foreach (var testUser in testUsers)
            {
                testUser.PasswordHash = _userManager.PasswordHasher.HashPassword(user: testUser, password: "password");
                testUser.User.AccountID = testUser.Id;
                testUser.User.Address.AccountID = testUser.Id;
                await _userManager.CreateAsync(testUser);

            }

            await _platformDb.SaveChangesAsync();
        }
    }
}

