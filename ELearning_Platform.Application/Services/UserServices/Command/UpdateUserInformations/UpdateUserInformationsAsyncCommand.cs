using ELearning_Platform.Domain.Models.UserAddress;
using MediatR;

namespace ELearning_Platform.Application.Services.UserServices.Command.UpdateUserInformations
{
    public class UpdateUserInformationsAsyncCommand : UpdateUserInformationsDto, IRequest<bool>
    {

    }
}
