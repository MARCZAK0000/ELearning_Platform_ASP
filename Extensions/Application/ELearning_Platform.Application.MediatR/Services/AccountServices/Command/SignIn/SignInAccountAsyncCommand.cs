using ELearning_Platform.Domain.Models.Models.AccountModel;
using ELearning_Platform.Domain.Models.Response.AccountResponse;
using MediatR;

namespace ELearning_Platform.Application.MediatR.Services.AccountServices.Command.SignIn
{
    public class SignInAsyncCommand : LoginModelDto, IRequest<LoginResponse>
    {

    }
}
