
using ELearning_Platform.Domain.Exceptions;
using ELearning_Platform.Domain.Exceptions.Exceptions;
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
            catch (BadRequestException err)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";
                var problemDetails = new ProblemDetails()
                {
                    Title = "Bad Request",
                    Type = "Client Error",
                    Status = (int)HttpStatusCode.BadRequest,
                    Detail = err.Message
                };
                var json = JsonSerializer.Serialize(problemDetails);
                await context.Response.WriteAsync(json);
            }

            catch (CredentialsAreInUsedException err)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";
                var problemDetails = new ProblemDetails()
                {
                    Title = "Invalid Credentials",
                    Type = "Client Error",
                    Status = (int)HttpStatusCode.BadRequest,
                    Detail = err.Message
                };
                var json = JsonSerializer.Serialize(problemDetails);
                await context.Response.WriteAsync(json);
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
            catch (NotFoundException err)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.ContentType = "application/json";
                var problemDetails = new ProblemDetails()
                {
                    Title = "Not Found",
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
            catch(InvalidRefreshTokenException err)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "application/json";
                var problemDetails = new ProblemDetails()
                {
                    Title = "Invalid Refresh Token",
                    Type = "Authorization Error",
                    Status = (int)HttpStatusCode.Unauthorized,
                    Detail = err.Message
                };
                var json = JsonSerializer.Serialize(problemDetails);
                await context.Response.WriteAsync(json);
            }

            catch (UnAuthorizedException err)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "application/json";
                var problemDetails = new ProblemDetails()
                {
                    Title = "Unauthorized",
                    Type = "Authorization Error",
                    Status = (int)HttpStatusCode.Unauthorized,
                    Detail = err.Message
                };
                var json = JsonSerializer.Serialize(problemDetails);
                await context.Response.WriteAsync(json);
            }
            catch (ForbidenException err)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                context.Response.ContentType = "application/json";
                var problemDetails = new ProblemDetails()
                {
                    Title = "Forbiden request",
                    Type = "Authorization Error",
                    Status = (int)HttpStatusCode.Forbidden,
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
