using ELearning_Platform.Domain.Models.Notification;
using ELearning_Platform.Domain.Models.Pagination;
using ELearning_Platform.Domain.Response.Notification;
using ELearning_Platform.Domain.Response.Pagination;

namespace ELearning_Platform.Domain.Repository
{
    public interface INotificaitonRepository
    {
        Task<bool> CreateNotificationAsync(CreateNotificationDto createNotification, CancellationToken token);

        Task<Pagination<GetNotificationModelDto>> ShowNotificationsAsync(PaginationModelDto pagination, CancellationToken token);

        Task<bool> ReadNotificationAsync(ReadNotificationDto readNotification, CancellationToken token);    
    }
}
