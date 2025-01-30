using ELearning_Platform.Domain.Database.Enitities;
using ELearning_Platform.Domain.Models.Models.Pagination;
using ELearning_Platform.Domain.Models.Models.UserAddress;
using ELearning_Platform.Domain.Models.Order;
using ELearning_Platform.Domain.Models.Response.Pagination;
using ELearning_Platform.Domain.Models.Response.UserReponse;
using Microsoft.AspNetCore.Http;

namespace ELearning_Platform.Domain.Core.Repository
{
    public interface IUserRepository
    {
        Task<GetUserInformationsDto> GetUserInformationsAsync(string userID, CancellationToken token);

        Task<User> GetOnlyUserInformationsAsync(string userID, CancellationToken token);

        Task<Pagination<GetUserInformationsDto>> GetAllUsersAsync(PaginationOrderModelDto<OrderByEnum> pagination, CancellationToken token);

        Task<bool> UpdateUserInfomrationsAsync(string userID, UpdateUserInformationsDto updateUserInformations, CancellationToken token);

        Task<bool> UpdateOrCreateImageProfile(string userID, IFormFile file, CancellationToken cancellationToken);
    }
}
