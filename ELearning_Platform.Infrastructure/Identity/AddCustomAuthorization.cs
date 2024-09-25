using ELearning_Platform.Infrastructure.AuthPolicy;
using Microsoft.Extensions.DependencyInjection;

namespace ELearning_Platform.Infrastructure.Identity
{
    public static class AddCustomAuthorization
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
