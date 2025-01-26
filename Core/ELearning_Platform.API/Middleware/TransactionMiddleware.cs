using ELearning_Platform.Application.CustomAttributes;
using ELearning_Platform.Infrastructure.Database.Database;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore.Storage;

namespace ELearning_Platform.API.Middleware
{
    public class TransactionMiddleware(PlatformDb platformDb) : IMiddleware
    {
        private readonly PlatformDb _platformDb = platformDb;
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Method.Equals("GET"))
            {
                await next.Invoke(context);
                return;
            }
            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            var attribute = endpoint?.Metadata.GetMetadata<TransactionAttribute>();
            if (attribute == null)
            {
                await next.Invoke(context);
                return;
            }

            IDbContextTransaction dbContextTransaction = null;
            try
            {
                dbContextTransaction = _platformDb.Database.BeginTransaction();
                await next.Invoke(context);
                dbContextTransaction.Commit();
            }
            catch (Exception)
            {
                dbContextTransaction?.Rollback();
            }
            finally
            {
                dbContextTransaction?.Dispose();
            }

        }
    }
}
