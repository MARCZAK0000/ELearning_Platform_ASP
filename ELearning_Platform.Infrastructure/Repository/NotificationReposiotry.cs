using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Exceptions;
using ELearning_Platform.Domain.Models.Notification;
using ELearning_Platform.Domain.Models.Pagination;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Response.Notification;
using ELearning_Platform.Domain.Response.Pagination;
using ELearning_Platform.Infrastructure.Authorization;
using ELearning_Platform.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace ELearning_Platform.Infrastructure.Repository
{
    public class NotificationReposiotry(PlatformDb platformDb, IUserContext userContext) : INotificaitonRepository
    {
        private readonly IUserContext _userContext = userContext;
        private readonly PlatformDb _platformDb = platformDb;

        public async Task<bool> CreateNotificationAsync(CreateNotificationDto createNotification, CancellationToken token)
        {
            var currentUser = _userContext.GetCurrentUser();

            var notification = new Notification()
            {
                Title = createNotification.Title,
                Description = createNotification.Describtion,
                RecipientID = createNotification.ReciverID,
                SenderID = currentUser.UserID,
            };

            await _platformDb.Notifications.AddAsync(entity:notification, cancellationToken: token);
            await _platformDb.SaveChangesAsync(cancellationToken: token);

            return true;
        }

     

        public async Task<Pagination<GetNotificationModelDto>> ShowNotificationsAsync(PaginationModelDto pagination, CancellationToken token)
        {
            if(pagination.PageIndex <= 0)
            {
                throw new BadRequestException("Invalid NotificationIndex");
            }

            var paginationBuilder = new PaginationBuilder<GetNotificationModelDto>();

            var currentUser = _userContext.GetCurrentUser();

            var findNotificationBase =
                _platformDb
                .Notifications
                .Include(pr => pr.Sender)
                .Where(pr => pr.IsUnread == true && pr.RecipientID == currentUser.UserID)
                .Select(pr => new GetNotificationModelDto()
                {
                    Title = pr.Title,
                    Description = pr.Description,
                    Sender = new GetNotificationSenderDto()
                    {
                        AccountID = pr.Sender.AccountID,
                        FirstName = pr.Sender.FirstName,
                        Surname = pr.Sender.Surname,

                    },
                    IsUnRead = false,
                    TimeSent = pr.TimeSent,
                });

            var count = await findNotificationBase.CountAsync(cancellationToken: token);

            var notifications = await findNotificationBase
                .Skip((pagination.PageIndex-1)*pagination.PageSize)
                .Take(pagination.PageSize)
                .OrderBy(pr=>pr.TimeSent)
                .ToListAsync(cancellationToken: token);

            return paginationBuilder
                .SetItems(notifications)
                .SetPageIndex(pagination.PageIndex)
                .SetPageSize(pagination.PageSize)
                .SetTotalCount(count)
                .SetFirstIndex(pageSize: pagination.PageSize, pageIndex: pagination.PageIndex)
                .SetLastIndex(pageSize: pagination.PageSize, pageIndex: pagination.PageIndex)
                .Build();                
        }
    }
}
