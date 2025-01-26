using ELearning_Platform.Domain.Database.Enitities;
using ELearning_Platform.Domain.Models.Models.ELearningTestModel;
using ELearning_Platform.Domain.Models.Response.Pagination;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ELearning_Platform.Application.MediatR.Services.ELearningTestServices.Query.GetTestsBySubjectID
{
    public class FindTestsBySubjectIDAsyncQuery : FindTestsBySomeIDDto,
        IRequest<Results<Ok<Pagination<Test>>, ForbidHttpResult, NotFound<ProblemDetails>>>
    {
    }
}
