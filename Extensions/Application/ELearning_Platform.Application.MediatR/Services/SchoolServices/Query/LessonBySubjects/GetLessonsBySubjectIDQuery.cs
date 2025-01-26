using ELearning_Platform.Domain.Database.Enitities;
using ELearning_Platform.Domain.Models.Models.SchoolModel;
using ELearning_Platform.Domain.Models.Response.Pagination;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ELearning_Platform.Application.MediatR.Services.SchoolServices.Query.LessonBySubjects
{
    public class GetLessonsBySubjectIDQuery : GetLessonsBySubjectIDDto, IRequest<Results<Ok<Pagination<Lesson>>, ForbidHttpResult>>
    {

    }
}
