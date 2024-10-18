using ELearning_Platform.Infrastructure.Authorization;
using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Exceptions;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using ELearning_Platform.Domain.Response.Pagination;
using ELearning_Platform.Domain.Response.UserReponse;
using ELearning_Platform.Domain.Models.Pagination;
using ELearning_Platform.Domain.Models.UserAddress;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Identity;
using ELearning_Platform.Domain.BackgroundTask;
using ELearning_Platform.Infrastructure.QueueService;
using ELearning_Platform.Infrastructure.BackgroundStrategy;

namespace ELearning_Platform.Infrastructure.Repository
{
    public class UserRepository(PlatformDb platformDb, 
        IUserContext userContext, BackgroundTask backgroundTask
        ,UserManager<Account> userManager, IImageHandlerQueue imageHandlerQueue
        ) : IUserRepository
    {
        private readonly PlatformDb _platformDb = platformDb;
        private readonly IUserContext _userContext = userContext;
        private readonly UserManager<Account> _userManager = userManager;
        private readonly IImageHandlerQueue _imageHandlerQueue = imageHandlerQueue;
        private readonly BackgroundTask _backgroundTask = backgroundTask;
        private readonly List<string> _extension = [".jpg", ".jpeg", ".png"];
        public async Task<GetUserInformationsDto> GetUserInformationsAsync(CancellationToken token)
        {
            var currentUser = _userContext.GetCurrentUser();
            var user = await _userManager.FindByIdAsync(currentUser.UserID);

            var informations = await _platformDb
                .UserInformations
                .Include(pr=>pr.Address)
                .Include(pr=>pr.Class)
                .AsSplitQuery() //to split query 
                .Where(pr=>pr.AccountID == currentUser.UserID)
                .Select(pr=> new GetUserInformationsDto
                {
                    AccountID = currentUser.UserID,
                    EmailAddress = pr.EmailAddress,
                    FirstName = pr.FirstName,
                    Surname = pr.Surname,
                    SecondName = pr.SecondName,
                    PhoneNumber = pr.PhoneNumber,
                    ClassName = pr.Class.Name,
                    Address = new UserAddressDto
                    {
                        City= pr.Address.City,
                        Country = pr.Address.Country,
                        StreetName= pr.Address.StreetName,
                        PostalCode = pr.Address.PostalCode
                    }
                     
                })
                .FirstOrDefaultAsync(token)??
                throw new InternalServerErrorException("Something went wrong");

            var roles = await _userManager.GetRolesAsync(user!);
            informations.RoleName = roles.Count>=1?roles[roles.Count-1]:roles[0];

            return informations;
        }

        public async Task<Pagination<GetUserInformationsDto>> GetAllUsersAsync(PaginationModelDto pagination, CancellationToken token)
        {
            var builder = new PaginationBuilder<GetUserInformationsDto>();

            var resultBase = _platformDb
                .UserInformations
                .Include(pr=>pr.Address)
                .Include(pr=>pr.Class)
                .AsSplitQuery()
                .Select(pr=>new GetUserInformationsDto
            {
                AccountID = pr.AccountID,
                EmailAddress = pr.EmailAddress,
                FirstName = pr.FirstName,
                SecondName= pr.SecondName,
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
            
            var result = await resultBase
                .Skip(((pagination.PageIndex-1)*pagination.PageSize))
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

        public async Task<bool> UpdateUserInfomrationsAsync(UpdateUserInformationsDto updateUserInformations, CancellationToken token)
        {
            var currentUser = _userContext.GetCurrentUser();

            var findUser = await _platformDb
                .UserInformations
                .Include(pr=>pr.Account)
                .Include(pr=>pr.Address)
                .Where(pr=>pr.AccountID == currentUser.UserID)
                .FirstOrDefaultAsync(cancellationToken: token) 
                ?? throw new NotFoundException("Not Found");

            
            findUser!.FirstName= updateUserInformations.FirstName;
            findUser.PhoneNumber = findUser.Account.PhoneNumber = updateUserInformations.PhoneNumber;
            findUser.Account.PhoneNumberConfirmed = false;
            findUser.Surname = updateUserInformations.Surname;
            findUser.Address.City = updateUserInformations.Address.City;
            findUser.Address.StreetName = updateUserInformations.Address.StreetName;
            findUser.Address.Country = updateUserInformations.Address.Country;
            findUser.Address.PostalCode = updateUserInformations.Address.PostalCode;
            findUser.Address.ModifiedDate = findUser.ModifiedDate = findUser.Account.ModifiedDate = DateTime.Now;
            if(findUser.Surname != null)findUser.SecondName = updateUserInformations.Surname;
            
            await _platformDb.SaveChangesAsync(cancellationToken: token);

            return true;
        }

        public async Task<bool> UpdateOrCreateImageProfile(IFormFile file, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser();

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
                        UserID = currentUser.UserID
                    }, token);
            });
            return await Task.FromResult(true);
        }
    }
}
