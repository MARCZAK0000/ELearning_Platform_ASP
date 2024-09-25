using ELearning_Platform.Domain.Models.AccountModel;
using MediatR;

namespace ELearning_Platform.Infrastructure.Services.AccountServices.Command.Register
{
    public class RegisterAccountAsyncCommand : RegisterModelDto, IRequest
    {

    }
}
