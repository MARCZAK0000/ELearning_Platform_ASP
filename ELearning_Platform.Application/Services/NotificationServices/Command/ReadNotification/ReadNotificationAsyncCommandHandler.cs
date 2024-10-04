using ELearning_Platform.Domain.Repository;
using MediatR;

namespace ELearning_Platform.Application.Services.NotificationServices.Command.ReadNotification
{
    public class ReadNotificationAsyncCommandHandler(INotificaitonRepository notificaiton) : IRequestHandler<ReadNotificationAsyncCommand, bool>
    {
        private readonly INotificaitonRepository _notificaiton = notificaiton;
        public async Task<bool> Handle(ReadNotificationAsyncCommand request, CancellationToken cancellationToken)
            => await _notificaiton.ReadNotificationAsync(request, cancellationToken);
    }
}
