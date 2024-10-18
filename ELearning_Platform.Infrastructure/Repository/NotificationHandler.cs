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
        EmailSettings emailSettings) : INotificationDecorator
    {
        private readonly NotificationSettings _notificationSettings = notificationSettings;
        private readonly IEmailSender _emailSender = emailSender;
        private readonly IEmailNotificationHandlerQueue _queue = queue;
        private readonly EmailSettings _emailSettings = emailSettings;

        public Task SendNotificaiton(List<CreateNotificationDto> notification, CancellationToken token)
        {
            if (_notificationSettings.EmailNotifications)
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
            return Task.CompletedTask;
        }
    }

    public class SMSNotification() : INotificationDecorator
    {
        public Task SendNotificaiton(List<CreateNotificationDto> notification, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }

    public class PushNotification() : INotificationDecorator
    {
        public Task SendNotificaiton(List<CreateNotificationDto> notification, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
