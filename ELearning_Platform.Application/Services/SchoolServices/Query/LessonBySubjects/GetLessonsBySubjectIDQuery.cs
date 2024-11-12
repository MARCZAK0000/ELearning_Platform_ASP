using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Models.SchoolModel;
using ELearning_Platform.Domain.Response.Pagination;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ELearning_Platform.Application.Services.SchoolServices.Query.LessonBySubjects
{
    public class GetLessonsBySubjectIDQuery : GetLessonsBySubjectIDDto, IRequest<Results<Ok<Pagination<Lesson>>, ForbidHttpResult>>
    {

    }
}
