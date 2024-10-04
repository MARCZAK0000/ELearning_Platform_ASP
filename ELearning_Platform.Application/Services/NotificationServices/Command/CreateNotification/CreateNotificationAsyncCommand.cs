using ELearning_Platform.Domain.Models.Notification;
using MediatR;

namespace ELearning_Platform.Application.Services.NotificationServices.Command.CreateNotification
{
    public class CreateNotificationAsyncCommand : CreateNotificationDto, IRequest<bool>
    {
    }
}
