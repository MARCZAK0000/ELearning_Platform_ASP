using ELearning_Platform.Domain.Response.UserReponse;
using MediatR;

namespace ELearning_Platform.Infrastructure.Services.UserServices.Queries.AllUserInformations
{
    public class GetInfromationsAboutAllUsersAsyncQuery : IRequest<List<GetUserInformationsDto>>
    {

    }
}
