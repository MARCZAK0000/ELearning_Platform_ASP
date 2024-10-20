using ELearning_Platform.Domain.BackgroundTask;
using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Models.Notification;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Response.Notification;
using ELearning_Platform.Domain.Settings;
using ELearning_Platform.Infrastructure.EmailSender.Class;
using ELearning_Platform.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace ELearning_Platform.Infrastructure.Repository
{
    public class EmailNotification(NotificationSettings notificationSettings,
        IEmailSender emailSender,
        IEmailNotificationHandlerQueue queue, 
        EmailSettings emailSettings, 
        INotificationDecorator notificationDecorator,
        INotificaitonRepository notificaitonRepository) : INotificationDecorator
    {
        private readonly NotificationSettings _notificationSettings = notificationSettings;
        private readonly IEmailSender _emailSender = emailSender;
        private readonly IEmailNotificationHandlerQueue _queue = queue;
        private readonly EmailSettings _emailSettings = emailSettings;
        private readonly INotificationDecorator _notificationDecorator = notificationDecorator;
        private readonly INotificaitonRepository _notificaitonRepository = notificaitonRepository;
        public async Task SendNotificaiton(List<string> list, CancellationToken token)
        {
            await _notificationDecorator.SendNotificaiton(list, token);
            if (_notificationSettings.EmailNotification)
            {
                List<GetNotificationModelDto> notifications = [];
                foreach (var item in list)
                {
                    var result = await 
                        _notificaitonRepository
                        .ShowNotification(item, token);
                    notifications.Add(result);
                }
                _queue.QueueBackgroundWorkItem(async token =>
                {
                    foreach (var item in notifications)
                    {
                        if(item.Sender != null)
                        {
                            await _emailSender.SendEmailAsync(new Domain.Email.EmailDto()
                            {
                                From = _emailSettings.Email,
                                Subject = item.Title,
                                To = item.Sender.Email!,
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
        public async Task SendNotificaiton(List<string> list, CancellationToken token)
        {
            await _notificationDecorator.SendNotificaiton(list, token);
            if (_notificationSettings.SMSNotification)
            {
                throw new NotImplementedException();
            }
            
        }
    }

    public class PushNotification(NotificationSettings notificationSettings, IHubContext<StronglyTypedNotificationHub
        , INotificationClient> hubContext, INotificaitonRepository notificaitonRepository) : INotificationDecorator
    {
        private readonly NotificationSettings _notificationSettings = notificationSettings;
        private readonly IHubContext<StronglyTypedNotificationHub, INotificationClient> _hubContext = hubContext;
        private readonly INotificaitonRepository _notificaitonRepository = notificaitonRepository;
        public async Task SendNotificaiton(List<string> list, CancellationToken token)
        {
            if (_notificationSettings.PushNotification)
            {
                List<GetNotificationModelDto> notifications = [];
                foreach (var item in list)
                {
                    var result = await
                        _notificaitonRepository
                        .ShowNotification(item, token);
                    notifications.Add(result);
                    await _hubContext.Clients.Clients(result.ReciverID).ReciveNotification(result);
                }
            }
        }
    }
}
