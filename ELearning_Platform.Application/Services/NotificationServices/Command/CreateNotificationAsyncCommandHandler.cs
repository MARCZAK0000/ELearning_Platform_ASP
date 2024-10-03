﻿using ELearning_Platform.Domain.Repository;
using MediatR;

namespace ELearning_Platform.Application.Services.NotificationServices.Command
{
    public class CreateNotificationAsyncCommandHandler(INotificaitonRepository notificaitonRepository)
        : IRequestHandler<CreateNotificationAsyncCommand, bool>
    {
        private readonly INotificaitonRepository _notificaitonRepository = notificaitonRepository;


        public async Task<bool> Handle(CreateNotificationAsyncCommand request, CancellationToken cancellationToken)
            => await _notificaitonRepository.CreateNotificationAsync(request, cancellationToken);
    }
}
