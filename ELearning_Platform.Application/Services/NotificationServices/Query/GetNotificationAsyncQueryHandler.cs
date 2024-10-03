using ELearning_Platform.Domain.Exceptions;
using ELearning_Platform.Domain.Models.Pagination;
using ELearning_Platform.Domain.Repository;
using ELearning_Platform.Domain.Response.Notification;
using ELearning_Platform.Domain.Response.Pagination;
using FluentValidation;
using MediatR;

namespace ELearning_Platform.Application.Services.NotificationServices.Query
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
