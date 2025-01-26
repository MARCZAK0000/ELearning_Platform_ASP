using ELearning_Platform.Domain.Models.Models.Notification;
using MediatR;

namespace ELearning_Platform.Application.MediatR.Services.NotificationServices.Command.CreateNotification
{
    public class CreateNotificationAsyncCommand : CreateNotificationDto, IRequest<bool>
    {
    }
}
