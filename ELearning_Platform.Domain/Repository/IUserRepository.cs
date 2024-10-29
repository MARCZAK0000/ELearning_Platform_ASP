using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Models.Pagination;
using ELearning_Platform.Domain.Models.UserAddress;
using ELearning_Platform.Domain.Response.Pagination;
using ELearning_Platform.Domain.Response.UserReponse;
using Microsoft.AspNetCore.Http;

namespace ELearning_Platform.Domain.Repository
{
    public interface IUserRepository
    {
        Task<GetUserInformationsDto> GetUserInformationsAsync(string userID, CancellationToken token);

        Task<Pagination<GetUserInformationsDto>> GetAllUsersAsync(PaginationModelDto pagination, CancellationToken token);

        Task<bool> UpdateUserInfomrationsAsync(string userID, UpdateUserInformationsDto updateUserInformations, CancellationToken token);

        Task<bool> UpdateOrCreateImageProfile(string userID, IFormFile file, CancellationToken cancellationToken);
    }
}
