using ELearning_Platform.Domain.Models.Models.UserAddress;
using MediatR;

namespace ELearning_Platform.Application.MediatR.Services.UserServices.Command.UpdateImage
{
    public class UpdateOrCreateImageProfileAsyncCommand : UserImageProfileDto, IRequest<bool>
    {
    }
}
