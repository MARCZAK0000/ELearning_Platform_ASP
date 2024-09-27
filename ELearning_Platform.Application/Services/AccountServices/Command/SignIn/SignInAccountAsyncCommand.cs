using ELearning_Platform.Domain.Models.AccountModel;
using ELearning_Platform.Domain.Response.AccountResponse;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ELearning_Platform.Application.Services.AccountServices.Command.SignIn
{
    public class SignInAsyncCommand : LoginModelDto, IRequest<bool> 
    {

    }
}
