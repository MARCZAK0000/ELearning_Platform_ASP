using ELearning_Platform.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ELearning_Platform.Application.Authorization
{
    public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public CurrentUser GetCurrentUser()
        {
            var user = _httpContextAccessor.HttpContext!.User;

            if (user == null || !user.Identity!.IsAuthenticated) 
            {
                throw new UnAuthorizedException("Authorized");
            }

            var userID = user.FindFirst(pr=>pr.Type == ClaimTypes.NameIdentifier)!.Value;

            var email = user.FindFirst(pr => pr.Type == ClaimTypes.Email)!.Value;

            return new CurrentUser(userID, email);   
        }
    }
}
