using ELearning_Platform.Domain.Models.UserAddress;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ELearning_Platform.Application.Services.UserServices.Command.UpdateImage
{
    public class UpdateOrCreateImageProfileAsyncCommand : UserImageProfileDto, IRequest<bool>
    {
    }
}
