using ELearning_Platform.Domain.Models.Pagination;
using ELearning_Platform.Domain.Response.Notification;
using ELearning_Platform.Domain.Response.Pagination;
using MediatR;

namespace ELearning_Platform.Application.Services.NotificationServices.Query.GetNotification
{
    public class GetNotificationsAsyncQuery : PaginationModelDto, IRequest<Pagination<GetNotificationModelDto>>
    {

    }
}
