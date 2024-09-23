using Microsoft.Extensions.DependencyInjection;

namespace ELearning_Platform.Application.AuthPolicy
{
    public static class AuthorizationContainer
    {
        public static void AddAuthorizationPolicy(this IServiceCollection services)
        {
            services.AddAuthorizationBuilder()
                .AddPolicy(PolicyConstant.RequireAdmin, PolicyFactory.RequireAdmin())
                .AddPolicy(PolicyConstant.RequireModerator, PolicyFactory.RequireModerator())
                .AddPolicy(PolicyConstant.RequireHeadTeacher, PolicyFactory.RequireHeadTeacher())
                .AddPolicy(PolicyConstant.RequireTeacher, PolicyFactory.ReqiureTeacher())
                .AddPolicy(PolicyConstant.RequireStudent, PolicyFactory.ReqiureStudent());
        }
    }
}
