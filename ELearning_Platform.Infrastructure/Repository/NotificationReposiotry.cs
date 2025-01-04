using Azure.Core;
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
    public class NotificationReposiotry(PlatformDb platformDb, 
        IUserContext userContext, INotificationGateway notificationGateway, IUserRepository userRepository): INotificaitonRepository
    {
        private readonly IUserContext _userContext = userContext;
        private readonly PlatformDb _platformDb = platformDb;
        private readonly INotificationGateway _notificationGateway = notificationGateway;
        private readonly IUserRepository userRepository = userRepository;

        public async Task<bool> CreateMoreThanOneNotificationAsync((string email, string userId) currentUser, List<CreateNotificationDto> list, CancellationToken token)
        {
            var sender = await userRepository.GetOnlyUserInformationsAsync(currentUser.userId, token);
            var notifications = list.Select(noti=>new Notification()
            {
                Title = noti.Title,
                Description = noti.Describtion,
                RecipientID = noti.ReciverID,
                Sender = sender,
            }).ToList();

            await _platformDb.Notifications.AddRangeAsync(notifications, token);
            await _platformDb.SaveChangesAsync(token);

            await _notificationGateway.HandleNotifications(currentUser, notifications, token);

            return true;
            
        }

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

        public async Task<bool> ReadNotificationAsync(ReadNotificationDto readNotification, CancellationToken token)
        {
            var user = _userContext.GetCurrentUser();
          
            var findNotification = await
                _platformDb
                .Notifications
                .Where(pr => pr.NotficaitonID == readNotification.NotificationID && pr.RecipientID == user.UserID)
                .ExecuteUpdateAsync((s => s.SetProperty(pr => pr.IsUnread, false)),token);

            return findNotification > 0;
        }

        public async Task<GetNotificationModelDto> ShowNotification(string notificationID, CancellationToken token)
        {
          

            return await _platformDb.Notifications
                .Where(pr => pr.NotficaitonID == notificationID)
                .Select(pr => new GetNotificationModelDto
                {
                    NotificationID = pr.NotficaitonID,
                    Title = pr.Title,
                    Description = pr.Description,
                    ReciverID = pr.RecipientID,
                    Sender = new GetNotificationSenderDto()
                    {
                        AccountID = pr.Sender.AccountID,
                        FirstName = pr.Sender.FirstName,
                        Surname = pr.Sender.Surname,
                        Email = pr.Sender.EmailAddress,
                    },
                    IsUnRead = pr.IsUnread,
                    TimeSent = pr.TimeSent,
                })
                .FirstOrDefaultAsync(token)??new GetNotificationModelDto();
        }


        /// <summary>
        /// For the purpose of this method <see cref="PaginationBuilder{T}.SetTotalCount(int)"/> is used for unread notifications
        /// </summary>
        public async Task<Pagination<GetNotificationModelDto>> ShowNotificationsAsync(PaginationModelDto pagination, CancellationToken token)
        {
          
            var paginationBuilder = new PaginationBuilder<GetNotificationModelDto>();

            var currentUser = _userContext.GetCurrentUser();

            var findNotificationBase =
                _platformDb
                .Notifications
                .Include(pr => pr.Sender)
                .Where(pr=>pr.RecipientID == currentUser.UserID)
                .Select(pr => new GetNotificationModelDto()
                {
                    NotificationID = pr.NotficaitonID,
                    Title = pr.Title,
                    Description = pr.Description,
                    ReciverID = pr.RecipientID,
                    Sender = new GetNotificationSenderDto()
                    {
                        AccountID = pr.Sender.AccountID,
                        FirstName = pr.Sender.FirstName,
                        Surname = pr.Sender.Surname,
                        Email = pr.Sender.EmailAddress,
                    },
                    IsUnRead = pr.IsUnread,
                    TimeSent = pr.TimeSent,
                }); 
            var count = await findNotificationBase.Where(pr=>pr.IsUnRead==true).CountAsync(cancellationToken: token);

            var notifications = await findNotificationBase
                .Skip((pagination.PageIndex-1)*pagination.PageSize)
                .Take(pagination.PageSize)
                .OrderByDescending(pr=>pr.IsUnRead==true)
                .ThenByDescending(pr=>pr.TimeSent)
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
