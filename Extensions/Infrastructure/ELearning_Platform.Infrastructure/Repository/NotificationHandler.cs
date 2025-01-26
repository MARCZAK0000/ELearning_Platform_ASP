using ELearning_Platform.Domain.Core.BackgroundTask;
using ELearning_Platform.Domain.Core.Repository;
using ELearning_Platform.Domain.Database.Enitities;
using ELearning_Platform.Domain.Models.Email;
using ELearning_Platform.Domain.Models.Response.Notification;
using ELearning_Platform.Domain.Setttings.Settings;
using ELearning_Platform.Infrastructure.Email.EmailSender.Class;
using ELearning_Platform.Infrastructure.Notifications.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ELearning_Platform.Infrastructure.Repository
{
    public class EmailNotification(NotificationSettings notificationSettings,
        IEmailSender emailSender,
        IEmailNotificationHandlerQueue queue,
        EmailSettings emailSettings,
        INotificationDecorator notificationDecorator) : INotificationDecorator
    {
        private readonly NotificationSettings _notificationSettings = notificationSettings;
        private readonly IEmailSender _emailSender = emailSender;
        private readonly IEmailNotificationHandlerQueue _queue = queue;
        private readonly EmailSettings _emailSettings = emailSettings;
        private readonly INotificationDecorator _notificationDecorator = notificationDecorator;
        public async Task SendNotificaiton((string email, string userID) currentUser, List<Notification> list, CancellationToken token)
        {
            await _notificationDecorator.SendNotificaiton(currentUser, list, token);
            if (_notificationSettings.EmailNotification)
            {
                _queue.QueueBackgroundWorkItem(async token =>
                {
                    if (currentUser.email != null)
                    {
                        foreach (var item in list)
                        {

                            await _emailSender.SendEmailAsync(new EmailDto()
                            {
                                From = _emailSettings.Email,
                                Subject = item.Title,
                                To = item.Recipient.EmailAddress,
                                Body = EmailSenderHelper.GenerateNotification(item.Title, item.Description)
                            }, token);

                        }
                    }
                });
            }

        }
    }
    public class SMSNotification(NotificationSettings notificationSettings
        , INotificationDecorator notificationDecorator)
        : INotificationDecorator
    {
        private readonly INotificationDecorator _notificationDecorator = notificationDecorator;
        private readonly NotificationSettings _notificationSettings = notificationSettings;
        public async Task SendNotificaiton((string email, string userID) currentUser, List<Notification> list, CancellationToken token)
        {
            await _notificationDecorator.SendNotificaiton(currentUser, list, token);
            if (_notificationSettings.SMSNotification)
            {
                throw new NotImplementedException();
            }

        }
    }
    public class PushNotification(NotificationSettings notificationSettings, IHubContext<StronglyTypedNotificationHub
        , INotificationClient> hubContext) : INotificationDecorator
    {
        private readonly NotificationSettings _notificationSettings = notificationSettings;
        private readonly IHubContext<StronglyTypedNotificationHub, INotificationClient> _hubContext = hubContext;
        //private readonly INotificaitonRepository _notificaitonRepository = notificaitonRepository;
        public async Task SendNotificaiton((string email, string userID) currentUser, List<Notification> list, CancellationToken token)
        {
            if (_notificationSettings.PushNotification)
            {
                foreach (var item in list)
                {

                    await _hubContext.Clients.Clients(item.RecipientID).ReciveNotification(new GetNotificationModelDto()
                    {
                        Title = item.Title,
                        Description = item.Description,
                        NotificationID = item.NotficaitonID,
                        IsUnRead = item.IsUnread,
                        Sender = new GetNotificationSenderDto()
                        {
                            AccountID = item.Sender.AccountID,
                            Email = item.Sender.EmailAddress,
                            FirstName = item.Sender.FirstName,
                            Surname = item.Sender.Surname
                        }
                    });
                }
            }
        }
    }
}
