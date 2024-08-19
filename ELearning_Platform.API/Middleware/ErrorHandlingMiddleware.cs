
using ELearning_Platform.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace ELearning_Platform.API.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
			try
			{
				await next.Invoke(context);
			}
            catch (InvalidEmailOrPasswordException err)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.ContentType = "application/json";
                var problemDetails = new ProblemDetails()
                {
                    Title = "InvalidCredentials",
                    Type = "Client Error",
                    Status = (int)HttpStatusCode.NotFound,
                    Detail = err.Message
                };
                var json = JsonSerializer.Serialize(problemDetails);
                await context.Response.WriteAsync(json);
            }
            catch (InternalServerErrorException err) 
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var problemDetails = new ProblemDetails()
                {
                    Title = "Internal Server Error",
                    Type = "Server error",
                    Status = (int)HttpStatusCode.InternalServerError,
                    Detail = err.Message
                };
                var json = JsonSerializer.Serialize(problemDetails);
                await context.Response.WriteAsync(json);
            }
			catch (Exception err)
			{
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var problemDetails = new ProblemDetails()
                {
                    Title = "Internal Server Error",
                    Type = "Server error",
                    Status = (int)HttpStatusCode.InternalServerError,
                    Detail = err.Message
                };
                var json = JsonSerializer.Serialize(problemDetails);
                await context.Response.WriteAsync(json);
                
			}
        }
    }
}
