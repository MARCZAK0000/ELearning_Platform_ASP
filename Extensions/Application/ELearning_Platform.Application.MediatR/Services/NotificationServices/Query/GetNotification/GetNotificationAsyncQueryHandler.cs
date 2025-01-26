using ELearning_Platform.Domain.Core.Repository;
using ELearning_Platform.Domain.Exceptions.Exceptions;
using ELearning_Platform.Domain.Models.Models.Pagination;
using ELearning_Platform.Domain.Models.Response.Notification;
using ELearning_Platform.Domain.Models.Response.Pagination;
using FluentValidation;
using MediatR;

namespace ELearning_Platform.Application.MediatR.Services.NotificationServices.Query.GetNotification
{
    public class GetNotificationAsyncQueryHandler(INotificaitonRepository notificaitonRepository
        , IValidator<PaginationModelDto> validator)
            : IRequestHandler<GetNotificationsAsyncQuery, Pagination<GetNotificationModelDto>>
    {
        private readonly INotificaitonRepository _notificaitonRepository = notificaitonRepository;
        private readonly IValidator<PaginationModelDto> _validator = validator;
        public async Task<Pagination<GetNotificationModelDto>> Handle(GetNotificationsAsyncQuery request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new BadRequestException(validationResult.Errors.ToString());
            }
            return await _notificaitonRepository.ShowNotificationsAsync(request, cancellationToken);
        }
    }
}
