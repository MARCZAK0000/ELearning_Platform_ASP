using Microsoft.AspNetCore.Authorization;

namespace ELearning_Platform.Application.AuthPolicy
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
                .RequireRole(Enum.GetNames(typeof(AuthorizationRole)).Take((int)AuthorizationRole.Admin))
                .Build();
        }

        public static AuthorizationPolicy RequireModerator()
        {
            return new AuthorizationPolicyBuilder()
                .RequireRole(Enum.GetNames(typeof(AuthorizationRole)).ToList().Take((int)AuthorizationRole.Moderator))
                .Build();
        }

        public static AuthorizationPolicy RequireHeadTeacher()
        {
            return new AuthorizationPolicyBuilder()
                .RequireRole(Enum.GetNames(typeof(AuthorizationRole)).ToList().Take((int)AuthorizationRole.HeadTeacher))
                .Build();
        }

        public static AuthorizationPolicy ReqiureTeacher()
        {
            return new AuthorizationPolicyBuilder()
                .RequireRole(Enum.GetNames(typeof(AuthorizationRole)).ToList().Take((int)AuthorizationRole.Teacher))
                .Build();
        }

        public static AuthorizationPolicy ReqiureStudent()
        {
            return new AuthorizationPolicyBuilder()
               .RequireRole(Enum.GetNames(typeof(AuthorizationRole)).ToList().Take((int)AuthorizationRole.Admin))
               .Build();
        }
    }




}
