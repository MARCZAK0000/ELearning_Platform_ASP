using ELearning_Platform.Domain.Models.Pagination;
using ELearning_Platform.Domain.Response.Pagination;
using ELearning_Platform.Domain.Response.UserReponse;
using MediatR;

namespace ELearning_Platform.Infrastructure.Services.UserServices.Queries.AllUserInformations
{
    public class GetInfromationsAboutAllUsersAsyncQuery : PaginationModelDto ,IRequest<Pagination<GetUserInformationsDto>>
    {

    }
}
