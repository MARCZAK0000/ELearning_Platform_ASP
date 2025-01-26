using ELearning_Platform.Domain.Models.Models.AccountModel;
using MediatR;

namespace ELearning_Platform.Application.MediatR.Services.AccountServices.Command.Register
{
    public class RegisterAccountAsyncCommand : RegisterModelDto, IRequest
    {

    }
}
