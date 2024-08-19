using ELearning_Platform.Domain.Models.AccountModel;
using ELearning_Platform.Domain.Response.Account;
using MediatR;

namespace ELearning_Platform.Application.Services.AccountServices.Command.SignIn
{
    public class SignInAsyncCommand : LoginModelDto ,IRequest<LoginResponse>
    {

    }
}
