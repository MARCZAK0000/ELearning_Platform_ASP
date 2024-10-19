using ELearning_Platform.Domain.BackgroundTask;
using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Models.Notification;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Settings;
using ELearning_Platform.Infrastructure.EmailSender.Class;

namespace ELearning_Platform.Infrastructure.Repository
{
    public class EmailNotification(NotificationSettings notificationSettings,
        IEmailSender emailSender,
        IEmailNotificationHandlerQueue queue, 
        EmailSettings emailSettings, INotificationDecorator notificationDecorator) : INotificationDecorator
    {
        private readonly NotificationSettings _notificationSettings = notificationSettings;
        private readonly IEmailSender _emailSender = emailSender;
        private readonly IEmailNotificationHandlerQueue _queue = queue;
        private readonly EmailSettings _emailSettings = emailSettings;
        private readonly INotificationDecorator _notificationDecorator = notificationDecorator;
        public async Task SendNotificaiton(List<CreateNotificationDto> notification, CancellationToken token)
        {
            await _notificationDecorator.SendNotificaiton(notification, token);
            if (_notificationSettings.EmailNotification)
            {
                _queue.QueueBackgroundWorkItem(async token =>
                {
                    foreach (var item in notification)
                    {
                        if(item.EmailAddress != null)
                        {
                            await _emailSender.SendEmailAsync(new Domain.Email.EmailDto()
                            {
                                From = _emailSettings.Email,
                                Subject = item.Title,
                                To = item.EmailAddress!,
                                Body = EmailSenderHelper.GenerateNotification(item.Title, item.Describtion)
                            }, token);
                        }
                    }
                });
            }
            
        }
    }

    public class SMSNotification(NotificationSettings notificationSettings, INotificationDecorator notificationDecorator) : INotificationDecorator
    {
        private readonly INotificationDecorator _notificationDecorator = notificationDecorator;
        private readonly NotificationSettings _notificationSettings = notificationSettings;
        public async Task SendNotificaiton(List<CreateNotificationDto> notification, CancellationToken token)
        {
            await _notificationDecorator.SendNotificaiton(notification, token);
            if (_notificationSettings.SMSNotification)
            {
                throw new NotImplementedException();
            }
            
        }
    }

    public class PushNotification(NotificationSettings notificationSettings) : INotificationDecorator
    {
        private readonly NotificationSettings _notificationSettings = notificationSettings;
        public async Task SendNotificaiton(List<CreateNotificationDto> notification, CancellationToken token)
        {
            if (_notificationSettings.PushNotification)
            {
                throw new NotImplementedException();
            }
        }
    }
}
