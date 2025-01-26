using ELearning_Platform.Domain.Database.Enitities;
using ELearning_Platform.Domain.Models.Models.SchoolModel;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ELearning_Platform.Application.MediatR.Services.SchoolServices.Query.LessonByID
{
    public class GetLessonByIDAsyncQuery : GetLessonByIDDto, IRequest<Results<Ok<Lesson>, NotFound<ProblemDetails>, ForbidHttpResult>>
    {
    }
}
