using ELearning_Platform.Domain.Models.ELearningTestModel;
using ELearning_Platform.Domain.Response.ElearningTest;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ELearning_Platform.Application.Services.ELearningTestServices.Query.GetTest
{
    public class GetTestByIDAsyncQuery : FindTestByIDDto,IRequest<Results<Ok<GetTestResponse>, NotFound<ProblemDetails>>>
    {
    }
}
