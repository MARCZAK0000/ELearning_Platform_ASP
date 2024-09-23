using ELearning_Platform.Application.Authorization;
using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Exceptions;
using ELearning_Platform.Domain.Helper;
using ELearning_Platform.Domain.Models.AccountModel;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Response.Account;
using ELearning_Platform.Infrastructure.Authentications;
using ELearning_Platform.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;

namespace ELearning_Platform.Infrastructure.Repository
{
    public class AccountRepository(SignInManager<Account> signInManager, PlatformDb platformDb,
        UserManager<Account> userManager, IUserContext userContext) : IAccountRepository
    {
        private readonly SignInManager<Account> _signInManager = signInManager;
        private readonly UserManager<Account> _userManager = userManager;
        private readonly PlatformDb _platformDb = platformDb;
        private readonly IUserContext _userContext = userContext;

        public async Task RegisterAccountAsync(RegisterModelDto registerModelDto, CancellationToken cancellationToken)
        {
            var checkIfExists = await _userManager.FindByEmailAsync(registerModelDto.AddressEmail);
            if (checkIfExists != null)
            {
                throw new InvalidEmailOrPasswordException("Email is in used");
            }

            var account = new Account()
            {
                Email = registerModelDto.AddressEmail,
                UserName = registerModelDto.AddressEmail,
                PhoneNumber = registerModelDto.PhoneNumber,
                User = new UserInformations()
                {
                    FirstName = registerModelDto.FirstName,
                    SecondName = registerModelDto.SecondName ?? string.Empty,
                    Surname = registerModelDto.Surname,
                    PhoneNumber = registerModelDto.PhoneNumber,
                    EmailAddress = registerModelDto.AddressEmail,
                    Address = new UserAddress()
                    {
                        City = registerModelDto.City,
                        Country = registerModelDto.Country,
                        StreetName = registerModelDto.StreetName,
                        PostalCode = registerModelDto.PostalCode,
                    }
                }
            };
            account.PasswordHash = _userManager.PasswordHasher.HashPassword(user: account, password: registerModelDto.Password);
            account.User.AccountID = account.Id;
            account.User.Address.AccountID = account.Id;

            await _userManager.CreateAsync(user: account);
            await _userManager.AddToRoleAsync(user: account, role: "student");
            await _platformDb.SaveChangesAsync(cancellationToken: cancellationToken);

        }

    }
}
