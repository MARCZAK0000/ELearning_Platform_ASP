using ELearning_Platform.Domain.Core.BackgroundTask;
using ELearning_Platform.Domain.Core.Repository;
using ELearning_Platform.Domain.Database.Enitities;
using ELearning_Platform.Domain.Exceptions.Exceptions;
using ELearning_Platform.Domain.Models.Helper;
using ELearning_Platform.Domain.Models.Models.AccountModel;
using ELearning_Platform.Domain.Models.Response.AccountResponse;
using ELearning_Platform.Domain.Setttings.Settings;
using ELearning_Platform.Infrastructure.BackgroundStrategy;
using ELearning_Platform.Infrastructure.Database.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ELearning_Platform.Infrastructure.Repository
{
    public class AccountRepository(SignInManager<Account> signInManager, PlatformDb platformDb,
        UserManager<Account> userManager,
        ITokenRepository tokenRepository, IEmailNotificationHandlerQueue backgroundTaskQueue,
        EmailSettings emailSettings, IEmailSenderHelper emailHelper, BackgroundTask backgroundTask) : IAccountRepository
    {
        private readonly SignInManager<Account> _signInManager = signInManager;
        private readonly UserManager<Account> _userManager = userManager;
        private readonly PlatformDb _platformDb = platformDb;
        private readonly IEmailNotificationHandlerQueue _backgroundTaskQueue = backgroundTaskQueue;
        private readonly ITokenRepository _tokenRepository = tokenRepository;
        private readonly EmailSettings _emailSettings = emailSettings;
        private readonly IEmailSenderHelper _emailHelper = emailHelper;
        private readonly BackgroundTask _backgroundTask = backgroundTask;

        public async Task RegisterAccountAsync(RegisterModelDto registerModelDto, CancellationToken cancellationToken)
        {
            var checkIfExists = await _userManager.FindByEmailAsync(registerModelDto.AddressEmail);
            if (checkIfExists != null)
            {
                throw new CredentialsAreInUsedException("Email is in used");
            }

            var account = new Account()
            {
                Email = registerModelDto.AddressEmail,
                UserName = registerModelDto.AddressEmail,
                PhoneNumber = registerModelDto.PhoneNumber,
                User = new User()
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

            var confrimEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(account);
            _backgroundTaskQueue.QueueBackgroundWorkItem(async token =>
            {
                await _backgroundTask.ExecuteTask(BackgroundEnum.Email,
                    _emailHelper.GenerateConfirmEmailMessage(registerModelDto.AddressEmail,
                    confrimEmailToken), token);
            });

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
                throw new InvalidEmailOrPasswordException(message: "Invalid Email or Password");
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
                Success = SignInResult.Success.Succeeded,
                Email = account.Email,
                Role = roles.Count >= 1 ? roles[0] : roles[roles.Count - 1],
                TokenModelDto = new TokenModelDto()
                {
                    AccessToken = token,
                    RefreshToken = account.RefreshToken,
                }
            };

        }



        public async Task<LoginResponse> RefreshTokenAsync(string userID, string refreshToken, CancellationToken cancellationToken)
        {

            var account = await _userManager.FindByIdAsync(userID);
            if (account!.RefreshToken != refreshToken)
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


                TokenModelDto = new TokenModelDto()
                {
                    AccessToken = token,
                    RefreshToken = account.RefreshToken,
                }
            };
        }

    }
}
