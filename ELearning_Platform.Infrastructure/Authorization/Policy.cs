using Microsoft.AspNetCore.Authorization;
using ELearning_Platform.Domain.Authorization;

namespace ELearning_Platform.Infrastructure.AuthPolicy
{
    public static class PolicyConstant
    {
        public const string RequireAdmin = "RequireAdmin";

        public const string RequireModerator = "RequireModerator";

        public const string RequireHeadTeacher = "RequireHeadTeacher";

        public const string RequireTeacher = "RequireTeacher";

        public const string RequireStudent = "RequireStudent";
    }

    public static class PolicyFactory
    {
        public static AuthorizationPolicy RequireAdmin()
        {
            
            return new AuthorizationPolicyBuilder()
                .RequireRole(Enum.GetNames(typeof(AuthorizationRole)).Take((int)AuthorizationRole.admin))
                .Build();
        }

        public static AuthorizationPolicy RequireModerator()
        {
            return new AuthorizationPolicyBuilder()
                .RequireRole(Enum.GetNames(typeof(AuthorizationRole)).ToList().Take((int)AuthorizationRole.moderator))
                .Build();
        }

        public static AuthorizationPolicy RequireHeadTeacher()
        {
            return new AuthorizationPolicyBuilder()
                .RequireRole(Enum.GetNames(typeof(AuthorizationRole)).ToList().Take((int)AuthorizationRole.headTeacher))
                .Build();
        }

        public static AuthorizationPolicy ReqiureTeacher()
        {
            return new AuthorizationPolicyBuilder()
                .RequireRole(Enum.GetNames(typeof(AuthorizationRole)).ToList().Take((int)AuthorizationRole.teacher))
                .Build();
        }

        public static AuthorizationPolicy ReqiureStudent()
        {
            return new AuthorizationPolicyBuilder()
               .RequireRole(Enum.GetNames(typeof(AuthorizationRole)).ToList().Take((int)AuthorizationRole.student))
               .Build();
        }
    }




}
