using ELearning_Platform.Domain.Response.UserReponse;
using MediatR;

namespace ELearning_Platform.Application.Services.UserServices.Queries.AllUserInformations
{
    public class GetInfromationsAboutAllUsersAsyncQuery : IRequest<List<GetUserInformationsDto>>
    {

    }
}
