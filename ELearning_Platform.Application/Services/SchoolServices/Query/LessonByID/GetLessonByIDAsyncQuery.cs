using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Models.SchoolModel;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ELearning_Platform.Application.Services.SchoolServices.Query.LessonByID
{
    public class GetLessonByIDAsyncQuery : GetLessonByIDDto, IRequest<Results<Ok<Lesson>, ForbidHttpResult>>
    {
    }
}
