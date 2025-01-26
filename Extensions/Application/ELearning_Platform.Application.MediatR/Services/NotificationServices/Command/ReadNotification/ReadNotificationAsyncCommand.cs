using ELearning_Platform.Domain.Models.Models.Notification;
using MediatR;

namespace ELearning_Platform.Application.MediatR.Services.NotificationServices.Command.ReadNotification
{
    public class ReadNotificationAsyncCommand : ReadNotificationDto, IRequest<bool>
    {
    }
}
