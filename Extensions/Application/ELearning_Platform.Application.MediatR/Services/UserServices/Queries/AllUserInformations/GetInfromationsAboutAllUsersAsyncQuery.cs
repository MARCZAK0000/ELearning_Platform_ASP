using ELearning_Platform.Domain.Models.Models.Pagination;
using ELearning_Platform.Domain.Models.Order;
using ELearning_Platform.Domain.Models.Response.Pagination;
using ELearning_Platform.Domain.Models.Response.UserReponse;
using MediatR;

namespace ELearning_Platform.Application.MediatR.Services.UserServices.Queries.AllUserInformations
{
    public class GetInfromationsAboutAllUsersAsyncQuery : PaginationOrderModelDto<OrderByEnum>, IRequest<Pagination<GetUserInformationsDto>>
    {

    }
}
