﻿using ELearning_Platform.Application.Authorization.Authorization;
using ELearning_Platform.Domain.Core.Repository;
using ELearning_Platform.Domain.Models.ErrorResponses;
using ELearning_Platform.Domain.Models.Models.Notification;
using ELearning_Platform.Domain.Setttings.Settings;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ELearning_Platform.Application.MediatR.Services.SchoolServices.Command.AddToClass
{
    public class AddToClassAsyncCommandHanlder
        (ISchoolRepository schoolRepository,
        INotificaitonRepository notificaitonRepository,
        IUserContext userContext)
        : IRequestHandler<AddToClassAsyncCommand, Results<Ok, ValidationProblem, NotFound<ProblemDetails>, ForbidHttpResult>>
    {
        private readonly ISchoolRepository _schoolRepository = schoolRepository;
        private readonly INotificaitonRepository _notificaitonRepository = notificaitonRepository;
        private readonly IUserContext _userContext = userContext;
        public async Task<Results<Ok, ValidationProblem, NotFound<ProblemDetails>, ForbidHttpResult>> Handle(AddToClassAsyncCommand request, CancellationToken cancellationToken)
        {

            var currentUser = _userContext.GetCurrentUser();

            if (!currentUser.IsInRole(nameof(AuthorizationRole.moderator))
                && !currentUser.IsInRole(nameof(AuthorizationRole.admin))
                    && !currentUser.IsInRole(nameof(AuthorizationRole.headTeacher)))
            {
                return TypedResults.Forbid(ErrorCodesResponse.ForbidError());
            }

            var getClass = await _schoolRepository.FindClassByClassIDAsync(request.ClassID, cancellationToken);
            if (getClass == null)
            {
                return TypedResults.NotFound(
                    ErrorCodesResponse.GenerateErrorResponse(ErrorCode.NotFound,
                    "Cannot Found Class By Class ID"));

            }

            var result = await _schoolRepository.AddStudentToClassAsync(getClass, request, cancellationToken);
            if (!result.IsSuccess)
                return TypedResults.ValidationProblem(ErrorCodesResponse
                        .ValidationProblemResponse("Cannot add students to database"));

            var getSubject = await _schoolRepository.FindSubjectByClassIDAsync(request.ClassID, cancellationToken);
            if (getSubject != null && getSubject.Count > 0)
            {
                var toClass = await _schoolRepository
                    .AddUsersToClassSubjectAsync(getSubject, request.UsersToAdd, cancellationToken);

                if (!toClass.IsSuccess)
                    return TypedResults.ValidationProblem(ErrorCodesResponse
                        .ValidationProblemResponse("problem with database"));
            }

            var notifications = new List<CreateNotificationDto>();

            foreach (var item in result.AddedUsers)
            {
                notifications.Add(new CreateNotificationDto
                {
                    Title = $"Add to class: ${item.ClassID}",
                    Describtion = "You have been add to class ",
                    ReciverID = item.AccountID,
                    EmailAddress = item.EmailAddress,
                    SenderID = currentUser.UserID,
                });
            }

            await _notificaitonRepository
                .CreateMoreThanOneNotificationAsync(
                    currentUser: (currentUser.EmailAddress, currentUser.UserID), notifications, cancellationToken);

            return TypedResults.Ok();
        }
    }
}
