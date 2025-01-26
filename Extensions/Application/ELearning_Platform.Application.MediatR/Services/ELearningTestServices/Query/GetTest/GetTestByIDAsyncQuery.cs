using ELearning_Platform.Domain.Models.Models.ELearningTestModel;
using ELearning_Platform.Domain.Models.Response.ElearningTest;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ELearning_Platform.Application.MediatR.Services.ELearningTestServices.Query.GetTest
{
    public class GetTestByIDAsyncQuery : FindTestByIDDto, IRequest<Results<Ok<GetTestResponse>, NotFound<ProblemDetails>>>
    {
    }
}
