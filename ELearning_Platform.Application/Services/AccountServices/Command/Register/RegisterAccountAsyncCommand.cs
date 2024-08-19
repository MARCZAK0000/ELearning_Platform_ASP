using ELearning_Platform.Domain.Models.AccountModel;
using MediatR;

namespace ELearning_Platform.Application.Services.AccountServices.Command.Register
{
    public class RegisterAccountAsyncCommand : RegisterModelDto, IRequest
    {

    }
}
