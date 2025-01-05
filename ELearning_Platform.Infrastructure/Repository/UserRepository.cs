using ELearning_Platform.Domain.BackgroundTask;
using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Exceptions;
using ELearning_Platform.Domain.Models.Pagination;
using ELearning_Platform.Domain.Models.UserAddress;
using ELearning_Platform.Domain.Order;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Response.Pagination;
using ELearning_Platform.Domain.Response.UserReponse;
using ELearning_Platform.Infrastructure.BackgroundStrategy;
using ELearning_Platform.Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ELearning_Platform.Infrastructure.Repository
{
    public class UserRepository(PlatformDb platformDb,
         BackgroundTask backgroundTask
        , UserManager<Account> userManager, IAzureHandlerQueue imageHandlerQueue
        ) : IUserRepository
    {
        private readonly PlatformDb _platformDb = platformDb;
        private readonly UserManager<Account> _userManager = userManager;
        private readonly IAzureHandlerQueue _imageHandlerQueue = imageHandlerQueue;
        private readonly BackgroundTask _backgroundTask = backgroundTask;
        private readonly List<string> _extension = [".jpg", ".jpeg", ".png"];
        public async Task<GetUserInformationsDto> GetUserInformationsAsync(string userID, CancellationToken token)
        {
            var user = await _userManager.FindByIdAsync(userID);

            var informations = await _platformDb
                .UserInformations
                .Include(pr => pr.Address)
                .Include(pr => pr.Class)
                .AsSplitQuery() //to split query 
                .Where(pr => pr.AccountID == userID)
                .Select(pr => new GetUserInformationsDto
                {
                    AccountID = userID,
                    EmailAddress = pr.EmailAddress,
                    FirstName = pr.FirstName,
                    Surname = pr.Surname,
                    SecondName = pr.SecondName,
                    PhoneNumber = pr.PhoneNumber,
                    ClassName = pr.Class.Name,
                    Address = new UserAddressDto
                    {
                        City = pr.Address.City,
                        Country = pr.Address.Country,
                        StreetName = pr.Address.StreetName,
                        PostalCode = pr.Address.PostalCode
                    }

                })
                .FirstOrDefaultAsync(token) ??
                throw new InternalServerErrorException("Something went wrong");

            var roles = await _userManager.GetRolesAsync(user!);
            informations.RoleName = roles.Count >= 1 ? roles[roles.Count - 1] : roles[0];

            return informations;
        }

        public async Task<Pagination<GetUserInformationsDto>> GetAllUsersAsync(PaginationModelDto pagination, CancellationToken token)
        {
            var builder = new PaginationBuilder<GetUserInformationsDto>();

            var resultBase = _platformDb
                .UserInformations
                .Include(pr => pr.Address)
                .Include(pr => pr.Class)
                .AsSplitQuery()
                .Select(pr => new GetUserInformationsDto
                {
                    AccountID = pr.AccountID,
                    EmailAddress = pr.EmailAddress,
                    FirstName = pr.FirstName,
                    SecondName = pr.SecondName,
                    Surname = pr.Surname,
                    PhoneNumber = pr.PhoneNumber,
                    ClassName = pr.Class!.Name,
                    Address = new Domain.Models.UserAddress.UserAddressDto()
                    {
                        City = pr.Address.City,
                        Country = pr.Address.Country,
                        PostalCode = pr.Address.PostalCode,
                        StreetName = pr.Address.StreetName,
                    },
                });
            var count = await resultBase.CountAsync(cancellationToken: token);

            var columnSelector = new Dictionary<OrderByEnum, Expression<Func<GetUserInformationsDto, object>>>
            {
                {   OrderByEnum.Surname, pr => pr.Surname},
                {   OrderByEnum.City, pr=>pr.Address.City},
                {   OrderByEnum.ClassName, pr=>pr.ClassName }

            };

            resultBase = pagination.IsDesc ?
                resultBase.OrderByDescending(columnSelector[pagination.OrderBy]) :
                resultBase.OrderBy(columnSelector[pagination.OrderBy]);

            var result = await resultBase
                .Skip(((pagination.PageIndex - 1) * pagination.PageSize))
                .Take(pagination.PageSize)
                .ToListAsync(cancellationToken: token);

            return builder
                .SetItems(items: result)
                .SetPageSize(pageSize: pagination.PageSize)
                .SetPageIndex(pageIndex: pagination.PageIndex)
                .SetFirstIndex(pageSize: pagination.PageSize, pageIndex: pagination.PageIndex)
                .SetLastIndex(pageSize: pagination.PageSize, pageIndex: pagination.PageIndex)
                .SetTotalCount(count)
                .Build();

        }

        public async Task<bool> UpdateUserInfomrationsAsync(string userID, UpdateUserInformationsDto updateUserInformations, CancellationToken token)
        {

            var findUser = await _platformDb
                .UserInformations
                .Include(pr => pr.Account)
                .Include(pr => pr.Address)
                .AsSplitQuery()
                .Where(pr => pr.AccountID == userID)
                .FirstOrDefaultAsync(cancellationToken: token)
                ?? throw new NotFoundException("Not Found");


            findUser!.FirstName = updateUserInformations.FirstName;
            findUser.PhoneNumber = findUser.Account.PhoneNumber = updateUserInformations.PhoneNumber;
            findUser.Account.PhoneNumberConfirmed = false;
            findUser.Surname = updateUserInformations.Surname;
            findUser.Address.City = updateUserInformations.Address.City;
            findUser.Address.StreetName = updateUserInformations.Address.StreetName;
            findUser.Address.Country = updateUserInformations.Address.Country;
            findUser.Address.PostalCode = updateUserInformations.Address.PostalCode;
            findUser.Address.ModifiedDate = findUser.ModifiedDate = findUser.Account.ModifiedDate = DateTime.Now;
            if (findUser.Surname != null) findUser.SecondName = updateUserInformations.Surname;

            await _platformDb.SaveChangesAsync(cancellationToken: token);

            return true;
        }

        public async Task<bool> UpdateOrCreateImageProfile(string userID, IFormFile file, CancellationToken cancellationToken)
        {
            if (!_extension.Contains(Path.GetExtension(file.FileName)))
            {
                throw new BadRequestException("Invalid image extension");
            }
            using var memoryStrem = new MemoryStream();
            file.CopyTo(memoryStrem);
            var data = memoryStrem.ToArray();
            _imageHandlerQueue.QueueBackgroundWorkItem(async token =>
            {
                await _backgroundTask.ExecuteTask(BackgroundEnum.Image,
                    new UpdateUserImageProfileDto
                    {
                        Image = data,
                        UserID = userID
                    }, token);
            });
            return await Task.FromResult(true);
        }

        public async Task<UserInformations> GetOnlyUserInformationsAsync(string userID, CancellationToken token)
        {
            return await _platformDb.UserInformations.Where(pr => pr.AccountID == userID)
                .FirstOrDefaultAsync(token) ?? throw new NotFoundException("Not FOund");
        }
    }
}
