using ELearning_Platform.Domain.Enitities;
using ELearning_Platform.Domain.Models.ELearningTestModel;
using ELearning_Platform.Domain.Response.Pagination;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ELearning_Platform.Application.Services.ELearningTestServices.Query.GetTestsBySubjectID
{
    public class FindTestsBySubjectIDAsyncQuery : FindTestsBySomeIDDto,
        IRequest<Results<Ok<Pagination<Test>>, ForbidHttpResult, NotFound<ProblemDetails>>>
    {
    }
}
