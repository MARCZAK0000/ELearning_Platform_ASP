using ELearning_Platform.Domain.Core.Repository;
using ELearning_Platform.Domain.Models.ErrorResponses;
using ELearning_Platform.Domain.Models.Response.ElearningTest;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ELearning_Platform.Application.MediatR.Services.ELearningTestServices.Query.GetTest
{
    public class GetTestByIDAsyncQueryHandler(IElearningTestRepository elearningTestRepository)
        : IRequestHandler<GetTestByIDAsyncQuery, Results<Ok<GetTestResponse>, NotFound<ProblemDetails>>>
    {
        private readonly IElearningTestRepository _elearningTestRepository = elearningTestRepository;

        public async Task<Results<Ok<GetTestResponse>, NotFound<ProblemDetails>>> Handle(GetTestByIDAsyncQuery request, CancellationToken cancellationToken)
        {
            var test = await _elearningTestRepository.FindTestByIdAsync(request.TestID, cancellationToken);

            if (test == null)
            {
                return TypedResults.NotFound(
                    ErrorCodesResponse.GenerateErrorResponse(ErrorCode.NotFound, "Invalid TestID")
                );
            }

            return TypedResults.Ok(new GetTestResponse
            {
                TestID = request.TestID,
                TestName = test.TestName,
                TestDescription = test.TestName,
            });
        }
    }
}
