using ELearning_Platform.Domain.Models.Notification;
using MediatR;

namespace ELearning_Platform.Application.Services.NotificationServices.Command.ReadNotification
{
    public class ReadNotificationAsyncCommand : ReadNotificationDto, IRequest<bool>
    {
    }
}
