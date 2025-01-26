using ELearning_Platform.Domain.Models.Models.UserAddress;
using MediatR;

namespace ELearning_Platform.Application.MediatR.Services.UserServices.Command.UpdateUserInformations
{
    public class UpdateUserInformationsAsyncCommand : UpdateUserInformationsDto, IRequest<bool>
    {

    }
}
