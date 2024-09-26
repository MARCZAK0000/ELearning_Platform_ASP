using ELearning_Platform.Domain.Authentication;
using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Exceptions;
using ELearning_Platform.Domain.Helper;
using ELearning_Platform.Domain.Models.AccountModel;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Response.AccountResponse;
using ELearning_Platform.Infrastructure.Authentications;
using ELearning_Platform.Infrastructure.Authorization;
using ELearning_Platform.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ELearning_Platform.Infrastructure.Repository
{
    public class AccountRepository(SignInManager<Account> signInManager, PlatformDb platformDb,
        UserManager<Account> userManager, IUserContext userContext,
        ITokenRepository tokenRepository) : IAccountRepository
    {
        private readonly SignInManager<Account> _signInManager = signInManager;
        private readonly UserManager<Account> _userManager = userManager;
        private readonly PlatformDb _platformDb = platformDb;
        private readonly IUserContext _userContext = userContext;
        
        private readonly ITokenRepository _tokenRepository = tokenRepository;

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

        public async Task<LoginResponse> SignInAsync(LoginModelDto loginModelDto, CancellationToken cancellationToken)
        {
            var account = await _userManager.FindByEmailAsync(email: loginModelDto.EmailAddress)
                ?? throw new InvalidEmailOrPasswordException(message: "Invalid Email or Password");

            var checkPassword = await _signInManager.PasswordSignInAsync(user: account, password: loginModelDto.Password,
                isPersistent: false, lockoutOnFailure: false);

            if (!checkPassword.Succeeded)
            {
                return new LoginResponse()
                {
                    Success = SignInResult.Failed
                };
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
            var token = await _tokenRepository.GenerateTokenAsync(tokenInformations: tokenInformations, roles: roles);

            account.RefreshToken = await GenerateRefreshToken.GenerateToken();

            await _platformDb.SaveChangesAsync(cancellationToken: cancellationToken);
            return new LoginResponse()
            {
                
                Success = SignInResult.Success,
                TokenModelDto = new TokenModelDto()
                {
                    AccessToken = token,
                    RefreshToken = account.RefreshToken,
                }
            };

        }

        

        public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenModelDto refreshToken, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();
            var account = await _userManager.FindByIdAsync(currentUser.UserID);
            if (account!.RefreshToken != refreshToken.RefreshToken)
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
            var token = await _tokenRepository.GenerateTokenAsync(tokenInformations: tokenInformations, roles: roles);

            account.RefreshToken = await GenerateRefreshToken.GenerateToken();

            await _platformDb.SaveChangesAsync(cancellationToken: cancellationToken);
            return new LoginResponse()
            {

                Success = SignInResult.Success,
                TokenModelDto = new TokenModelDto()
                {
                    AccessToken = token,
                    RefreshToken = account.RefreshToken,
                }
            };
        }

    }
}
