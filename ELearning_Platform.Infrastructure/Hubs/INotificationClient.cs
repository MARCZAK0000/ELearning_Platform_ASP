﻿using ELearning_Platform.Domain.Response.Notification;

namespace ELearning_Platform.Infrastructure.Hubs
{
    public interface INotificationClient
    {
        Task ReciveMessage(GetNotificationModelDto getNotification);
    }
}