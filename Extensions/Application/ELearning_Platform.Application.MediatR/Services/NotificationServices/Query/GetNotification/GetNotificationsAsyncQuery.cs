using ELearning_Platform.Domain.Models.Models.Pagination;
using ELearning_Platform.Domain.Models.Response.Notification;
using ELearning_Platform.Domain.Models.Response.Pagination;
using MediatR;

namespace ELearning_Platform.Application.MediatR.Services.NotificationServices.Query.GetNotification
{
    public class GetNotificationsAsyncQuery : PaginationModelDto, IRequest<Pagination<GetNotificationModelDto>>
    {

    }
}
