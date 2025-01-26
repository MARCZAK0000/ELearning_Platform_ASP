using ELearning_Platform.Domain.Models.Models.Notification;
using ELearning_Platform.Domain.Models.Models.Pagination;
using ELearning_Platform.Domain.Models.Response.Notification;
using ELearning_Platform.Domain.Models.Response.Pagination;

namespace ELearning_Platform.Domain.Core.Repository
{
    public interface INotificaitonRepository
    {
        Task<bool> CreateNotificationAsync(CreateNotificationDto createNotification, CancellationToken token);

        Task<bool> CreateMoreThanOneNotificationAsync((string email, string userId) currentUser, List<CreateNotificationDto> list, CancellationToken token);

        Task<Pagination<GetNotificationModelDto>> ShowNotificationsAsync(PaginationModelDto pagination, CancellationToken token);

        Task<GetNotificationModelDto> ShowNotification(string notificationID, CancellationToken token);

        Task<bool> ReadNotificationAsync(ReadNotificationDto readNotification, CancellationToken token);
    }
}
