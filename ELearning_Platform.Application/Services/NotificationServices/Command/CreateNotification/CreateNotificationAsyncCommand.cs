using ELearning_Platform.Domain.Models.Notification;
using MediatR;

namespace ELearning_Platform.Application.Services.NotificationServices.Command
{
    public class CreateNotificationAsyncCommand : CreateNotificationDto, IRequest<bool>
    {
    }
}
