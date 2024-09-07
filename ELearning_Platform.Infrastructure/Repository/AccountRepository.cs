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
        UserManager<Account> userManager, AuthenticationSettings authenticationSettings, IUserContext userContext) : IAccountRepository
    {
        private readonly SignInManager<Account> _signInManager = signInManager;
        private readonly UserManager<Account> _userManager = userManager;
        private readonly PlatformDb _platformDb = platformDb;
        private readonly AuthenticationSettings _authenticationSettings = authenticationSettings;
        private readonly IUserContext _userContext = userContext;

        public async Task RegisterAccountAsync(RegisterModelDto registerModelDto, CancellationToken cancellationToken)
        {
            var checkIfExists = await _userManager.FindByEmailAsync(registerModelDto.AddressEmail);
            if(checkIfExists != null)
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

        public async Task<LoginResponse> SignInAsync(LoginModelDto loginModelDto, CancellationToken cancellationToken)
        {
            var account = await _userManager.FindByEmailAsync(email: loginModelDto.EmailAddress) 
                ?? throw new InvalidEmailOrPasswordException(message:"Invalid Email or Password");

            var checkPassword = await _signInManager.PasswordSignInAsync(user: account, password: loginModelDto.Password,
                isPersistent: false, lockoutOnFailure: false);

            if (!checkPassword.Succeeded)
            {
                throw new InvalidEmailOrPasswordException(message: "Invalid Email or Password");
            }

            var tokenInformations = await _platformDb
                .UserInformations
                .Include(pr=>pr.Address)
                .Where(pr=>pr.AccountID == account.Id)
                .Select(pr=>new ClaimsInformations()
                {
                    AccountId = pr.AccountID,
                    Email = pr.EmailAddress, 
                    PhoneNumber = pr.PhoneNumber,
                    Surname = pr.Surname,
                    City = pr.Address.City,
                    Country = pr.Address.Country
                })
                .FirstOrDefaultAsync(cancellationToken: cancellationToken)??
                throw new InternalServerErrorException(message: "There was a problem on server side");

            var roles = await _userManager.GetRolesAsync(user: account);
            var token = await GenerateTokenAsync(tokenInformations: tokenInformations, roles: roles);

            account.RefreshToken = await GenerateRefreshToken.GenerateToken();

            await _platformDb.SaveChangesAsync(cancellationToken: cancellationToken);
            return new LoginResponse()
            {
                Email = loginModelDto.EmailAddress,
                Token = token,
                RefreshToken = account.RefreshToken,
            };

        }

        public Task<string> GenerateTokenAsync(ClaimsInformations tokenInformations, IList<string> roles)
        {
            var claims = new List<Claim>()
            {
                new(type: ClaimTypes.NameIdentifier, value: tokenInformations.AccountId),
                new(type: ClaimTypes.Email, value: tokenInformations.Email),
                new(type: ClaimTypes.HomePhone, value: tokenInformations.PhoneNumber),
                new(type: ClaimTypes.Surname, value: tokenInformations.Surname),
                new(type: ClaimTypes.Country, value: tokenInformations.Country),
                new(type: "City", value: tokenInformations.City),
            };

            foreach (var item in roles)
            {
                claims.Add(
                    new Claim(type: ClaimTypes.Role, value: item));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.Key));
            var credentails = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expireDays = DateTime.Now.AddMinutes(_authenticationSettings.ExpireMinutes);
            
            var token = new JwtSecurityToken(
                issuer: _authenticationSettings.Issure, 
                audience:_authenticationSettings.Key, 
                claims: claims,
                notBefore: null,
                expires: expireDays,
                credentails);

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        } 

        public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenModelDto refreshToken, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            var account = await _userManager.FindByIdAsync(currentUser.UserID);
            if(account!.RefreshToken != refreshToken.RefreshToken)
            {
                throw new InvalidRefreshTokenException("Invalid Refresh Token");
            }


            var tokenInformations = await _platformDb
               .UserInformations
               .Include(pr => pr.Address)
               .Where(pr => pr.AccountID == account.Id)
               .Select(pr => new ClaimsInformations()
               {
                   AccountId = pr.AccountID,
                   Email = pr.EmailAddress,
                   PhoneNumber = pr.PhoneNumber,
                   Surname = pr.Surname,
                   City = pr.Address.City,
                   Country = pr.Address.Country
               })
               .FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
               throw new InternalServerErrorException(message: "There was a problem on server side");

            var roles = await _userManager.GetRolesAsync(user: account);
            var token = await GenerateTokenAsync(tokenInformations: tokenInformations, roles: roles);

            account.RefreshToken = await GenerateRefreshToken.GenerateToken();

            await _platformDb.SaveChangesAsync(cancellationToken: cancellationToken);
            return new LoginResponse()
            {
                Email = account.Email!,
                Token = token,
                RefreshToken = account.RefreshToken,
            };
        }
    }
}
