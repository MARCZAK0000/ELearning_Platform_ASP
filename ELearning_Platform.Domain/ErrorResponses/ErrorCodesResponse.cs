using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ELearning_Platform.Domain.ErrorResponses
{
    public static class ErrorCodesResponse
    {
        public static ProblemDetails GenerateErrorResponse(ErrorCode errorCode, string Details)
        {
            return new ProblemDetails
            {
                Title = (int)errorCode == 400 ? "Bad Request" :
                            (int)errorCode == 401 ? "UnAuthorized" :
                                (int)errorCode == 403 ? "Forbid" :
                                    "NotFound",
                Status = (int)errorCode,
                Detail = Details,
            };
        }

        public static AuthenticationProperties ForbidError()
        {
            return new AuthenticationProperties
            {
                IssuedUtc = DateTime.UtcNow,
            };
        }

        public static Dictionary<string, string[]> ValidationProblemResponse(string title)
        {
            return new Dictionary<string, string[]>
                {
                    {"error", [title] }
                };
        }
    }


    public enum ErrorCode
    {
        BadRequest = 400,
        Unauthorized = 401,
        Forbid = 403,
        NotFound = 404
    }
}

