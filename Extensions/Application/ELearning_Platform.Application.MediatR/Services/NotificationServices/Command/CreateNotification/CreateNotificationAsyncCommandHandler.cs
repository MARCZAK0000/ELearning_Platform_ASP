using ELearning_Platform.Domain.Core.Repository;
using MediatR;

namespace ELearning_Platform.Application.MediatR.Services.NotificationServices.Command.CreateNotification
{
    public class CreateNotificationAsyncCommandHandler(INotificaitonRepository notificaitonRepository)
        : IRequestHandler<CreateNotificationAsyncCommand, bool>
    {
        private readonly INotificaitonRepository _notificaitonRepository = notificaitonRepository;


        public async Task<bool> Handle(CreateNotificationAsyncCommand request, CancellationToken cancellationToken)
            => await _notificaitonRepository.CreateNotificationAsync(request, cancellationToken);
    }
}
