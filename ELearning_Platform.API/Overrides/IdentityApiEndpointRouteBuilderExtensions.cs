﻿using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace ELearning_Platform.API.Overrides
{
    public static class IdentityApiEndpointRouteBuilderExtensions
    {
        // Validate the email address using DataAnnotations like the UserValidator does when RequireUniqueEmail = true.
        private static readonly EmailAddressAttribute _emailAddressAttribute = new();

        public static IEndpointConventionBuilder MapIdentityApiFilterable<TUser>(this IEndpointRouteBuilder endpoints,
            IdentityApiEndpointRouteBuilderOptions configureOptions)
            where TUser : class, new()
        {
            ArgumentNullException.ThrowIfNull(endpoints);

            var timeProvider = endpoints.ServiceProvider.GetRequiredService<TimeProvider>();
            var bearerTokenOptions = endpoints.ServiceProvider.GetRequiredService<IOptionsMonitor<BearerTokenOptions>>();
            var emailSender = endpoints.ServiceProvider.GetRequiredService<IEmailSender<TUser>>();
            var linkGenerator = endpoints.ServiceProvider.GetRequiredService<LinkGenerator>();

            string? confirmEmailEndpointName = null;

            var routeGroup = endpoints.MapGroup("/api/account");

            if (configureOptions.ExcludeRegisterPost)
            {

                routeGroup.MapPost("/register", async Task<Results<Ok, ValidationProblem>>
                    ([FromBody] RegisterRequest registration, HttpContext context, [FromServices] IServiceProvider sp) =>
                {
                    var userManager = sp.GetRequiredService<UserManager<TUser>>();

                    if (!userManager.SupportsUserEmail)
                    {
                        throw new NotSupportedException($"{nameof(MapIdentityApiFilterable)} requires a user store with email support.");
                    }

                    var userStore = sp.GetRequiredService<IUserStore<TUser>>();
                    var emailStore = (IUserEmailStore<TUser>)userStore;
                    var email = registration.Email;

                    if (string.IsNullOrEmpty(email) || !_emailAddressAttribute.IsValid(email))
                    {
                        return CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(email)));
                    }

                    var user = new TUser();
                    await userStore.SetUserNameAsync(user, email, CancellationToken.None);
                    await emailStore.SetEmailAsync(user, email, CancellationToken.None);
                    var result = await userManager.CreateAsync(user, registration.Password);

                    if (!result.Succeeded)
                    {
                        return CreateValidationProblem(result);
                    }

                    await SendConfirmationEmailAsync(user, userManager, context, email);
                    return TypedResults.Ok();
                });
            }

            if (configureOptions.ExcludeLoginPost)
            {
                routeGroup.MapPost("/login", async Task<Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>>
                ([FromBody] LoginRequest login, [FromQuery] bool? useCookies, [FromQuery] bool? useSessionCookies, [FromServices] IServiceProvider sp) =>
                {
                    var httpContext = sp.GetService<HttpContext>();
                    var signInManager = sp.GetRequiredService<SignInManager<TUser>>();
                    var result = await SignInAsync<TUser>(signInManager, login, sp);
                    return result;
                });
            }

            if (configureOptions.ExcludeRefreshPost)
            {
                routeGroup.MapPost("/refresh", async Task<Results<Ok<AccessTokenResponse>, UnauthorizedHttpResult, SignInHttpResult, ChallengeHttpResult>>
                ([FromBody] RefreshRequest refreshRequest, [FromServices] IServiceProvider sp) =>
                {
                    var signInManager = sp.GetRequiredService<SignInManager<TUser>>();
                    var refreshTokenProtector = bearerTokenOptions.Get(IdentityConstants.BearerScheme).RefreshTokenProtector;
                    var refreshTicket = refreshTokenProtector.Unprotect(refreshRequest.RefreshToken);

                    if (refreshTicket?.Properties?.ExpiresUtc is not { } expiresUtc ||
                        timeProvider.GetUtcNow() >= expiresUtc ||
                        await signInManager.ValidateSecurityStampAsync(refreshTicket.Principal) is not TUser user)

                    {
                        return TypedResults.Challenge();
                    }

                    var newPrincipal = await signInManager.CreateUserPrincipalAsync(user);
                    return TypedResults.SignIn(newPrincipal, authenticationScheme: IdentityConstants.BearerScheme);
                });
            }

            if (configureOptions.ExcludeConfirmEmailGet)
            {
                routeGroup.MapGet("/confirmEmail", async Task<Results<ContentHttpResult, UnauthorizedHttpResult>>
                ([FromQuery] string userId, [FromQuery] string code, [FromQuery] string? changedEmail, [FromServices] IServiceProvider sp) =>
                {
                    var userManager = sp.GetRequiredService<UserManager<TUser>>();
                    if (await userManager.FindByIdAsync(userId) is not { } user)
                    {
                        return TypedResults.Unauthorized();
                    }

                    try
                    {
                        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                    }
                    catch (FormatException)
                    {
                        return TypedResults.Unauthorized();
                    }

                    IdentityResult result;

                    if (string.IsNullOrEmpty(changedEmail))
                    {
                        result = await userManager.ConfirmEmailAsync(user, code);
                    }
                    else
                    {

                        result = await userManager.ChangeEmailAsync(user, changedEmail, code);

                        if (result.Succeeded)
                        {
                            result = await userManager.SetUserNameAsync(user, changedEmail);
                        }
                    }

                    if (!result.Succeeded)
                    {
                        return TypedResults.Unauthorized();
                    }

                    return TypedResults.Text("Thank you for confirming your email.");
                })
            .Add(endpointBuilder =>
            {
                var finalPattern = ((RouteEndpointBuilder)endpointBuilder).RoutePattern.RawText;
                confirmEmailEndpointName = $"{nameof(MapIdentityApiFilterable)}-{finalPattern}";
                endpointBuilder.Metadata.Add(new EndpointNameMetadata(confirmEmailEndpointName));
            });
            }

            if (configureOptions.ExcludeResendConfirmationEmailPost)
            {
                routeGroup.MapPost("/resendConfirmationEmail", async Task<Ok>
                ([FromBody] ResendConfirmationEmailRequest resendRequest, HttpContext context, [FromServices] IServiceProvider sp) =>
                {
                    var userManager = sp.GetRequiredService<UserManager<TUser>>();
                    if (await userManager.FindByEmailAsync(resendRequest.Email) is not { } user)
                    {
                        return TypedResults.Ok();
                    }

                    await SendConfirmationEmailAsync(user, userManager, context, resendRequest.Email);
                    return TypedResults.Ok();
                });
            }

            if (configureOptions.ExcludeForgotPasswordPost)
            {
                routeGroup.MapPost("/forgotPassword", async Task<Results<Ok, ValidationProblem>>
                ([FromBody] ForgotPasswordRequest resetRequest, [FromServices] IServiceProvider sp) =>
                {
                    var userManager = sp.GetRequiredService<UserManager<TUser>>();
                    var user = await userManager.FindByEmailAsync(resetRequest.Email);

                    if (user is not null && await userManager.IsEmailConfirmedAsync(user))
                    {
                        var code = await userManager.GeneratePasswordResetTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                        await emailSender.SendPasswordResetCodeAsync(user, resetRequest.Email, HtmlEncoder.Default.Encode(code));
                    }


                    return TypedResults.Ok();
                });
            }

            if (configureOptions.ExcludeResetPasswordPost)
            {
                routeGroup.MapPost("/resetPassword", async Task<Results<Ok, ValidationProblem>>
                ([FromBody] ResetPasswordRequest resetRequest, [FromServices] IServiceProvider sp) =>
                {
                    var userManager = sp.GetRequiredService<UserManager<TUser>>();

                    var user = await userManager.FindByEmailAsync(resetRequest.Email);

                    if (user is null || !await userManager.IsEmailConfirmedAsync(user))
                    {

                        return CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.InvalidToken()));
                    }

                    IdentityResult result;
                    try
                    {
                        var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetRequest.ResetCode));
                        result = await userManager.ResetPasswordAsync(user, code, resetRequest.NewPassword);
                    }
                    catch (FormatException)
                    {
                        result = IdentityResult.Failed(userManager.ErrorDescriber.InvalidToken());
                    }

                    if (!result.Succeeded)
                    {
                        return CreateValidationProblem(result);
                    }

                    return TypedResults.Ok();
                });
            }

            if (configureOptions.ExcludeManageGroup)
            {
                var accountGroup = routeGroup.MapGroup("/manage").RequireAuthorization();

                if (configureOptions.Exclude2faPost)
                {
                    accountGroup.MapPost("/2fa", async Task<Results<Ok<TwoFactorResponse>, ValidationProblem, NotFound>>
                    (ClaimsPrincipal claimsPrincipal, [FromBody] TwoFactorRequest tfaRequest, [FromServices] IServiceProvider sp) =>
                    {
                        var signInManager = sp.GetRequiredService<SignInManager<TUser>>();
                        var userManager = signInManager.UserManager;
                        if (await userManager.GetUserAsync(claimsPrincipal) is not { } user)
                        {
                            return TypedResults.NotFound();
                        }

                        if (tfaRequest.Enable == true)
                        {
                            if (tfaRequest.ResetSharedKey)
                            {
                                return CreateValidationProblem("CannotResetSharedKeyAndEnable",
                                    "Resetting the 2fa shared key must disable 2fa until a 2fa token based on the new shared key is validated.");
                            }
                            else if (string.IsNullOrEmpty(tfaRequest.TwoFactorCode))
                            {
                                return CreateValidationProblem("RequiresTwoFactor",
                                    "No 2fa token was provided by the request. A valid 2fa token is required to enable 2fa.");
                            }
                            else if (!await userManager.VerifyTwoFactorTokenAsync(user, userManager.Options.Tokens.AuthenticatorTokenProvider, tfaRequest.TwoFactorCode))
                            {
                                return CreateValidationProblem("InvalidTwoFactorCode",
                                    "The 2fa token provided by the request was invalid. A valid 2fa token is required to enable 2fa.");
                            }

                            await userManager.SetTwoFactorEnabledAsync(user, true);
                        }
                        else if (tfaRequest.Enable == false || tfaRequest.ResetSharedKey)
                        {
                            await userManager.SetTwoFactorEnabledAsync(user, false);
                        }

                        if (tfaRequest.ResetSharedKey)
                        {
                            await userManager.ResetAuthenticatorKeyAsync(user);
                        }

                        string[]? recoveryCodes = null;
                        if (tfaRequest.ResetRecoveryCodes || tfaRequest.Enable == true && await userManager.CountRecoveryCodesAsync(user) == 0)
                        {
                            var recoveryCodesEnumerable = await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
                            recoveryCodes = recoveryCodesEnumerable?.ToArray();
                        }

                        if (tfaRequest.ForgetMachine)
                        {
                            await signInManager.ForgetTwoFactorClientAsync();
                        }

                        var key = await userManager.GetAuthenticatorKeyAsync(user);
                        if (string.IsNullOrEmpty(key))
                        {
                            await userManager.ResetAuthenticatorKeyAsync(user);
                            key = await userManager.GetAuthenticatorKeyAsync(user);

                            if (string.IsNullOrEmpty(key))
                            {
                                throw new NotSupportedException("The user manager must produce an authenticator key after reset.");
                            }
                        }

                        return TypedResults.Ok(new TwoFactorResponse
                        {
                            SharedKey = key,
                            RecoveryCodes = recoveryCodes,
                            RecoveryCodesLeft = recoveryCodes?.Length ?? await userManager.CountRecoveryCodesAsync(user),
                            IsTwoFactorEnabled = await userManager.GetTwoFactorEnabledAsync(user),
                            IsMachineRemembered = await signInManager.IsTwoFactorClientRememberedAsync(user),
                        });
                    });
                }

                if (configureOptions.ExcludegInfoGet)
                {

                    accountGroup.MapGet("/info", async Task<Results<Ok<InfoResponse>, ValidationProblem, NotFound>>
                    (ClaimsPrincipal claimsPrincipal, [FromServices] IServiceProvider sp) =>
                    {
                        var userManager = sp.GetRequiredService<UserManager<TUser>>();
                        if (await userManager.GetUserAsync(claimsPrincipal) is not { } user)
                        {
                            return TypedResults.NotFound();
                        }

                        return TypedResults.Ok(await CreateInfoResponseAsync(user, userManager));
                    });
                }

                if (configureOptions.ExcludeInfoPost)
                {
                    accountGroup.MapPost("/info", async Task<Results<Ok<InfoResponse>, ValidationProblem, NotFound>>
                    (ClaimsPrincipal claimsPrincipal, [FromBody] InfoRequest infoRequest, HttpContext context, [FromServices] IServiceProvider sp) =>
                    {
                        var userManager = sp.GetRequiredService<UserManager<TUser>>();
                        if (await userManager.GetUserAsync(claimsPrincipal) is not { } user)
                        {
                            return TypedResults.NotFound();
                        }

                        if (!string.IsNullOrEmpty(infoRequest.NewEmail) && !_emailAddressAttribute.IsValid(infoRequest.NewEmail))
                        {
                            return CreateValidationProblem(IdentityResult.Failed(userManager.ErrorDescriber.InvalidEmail(infoRequest.NewEmail)));
                        }

                        if (!string.IsNullOrEmpty(infoRequest.NewPassword))
                        {
                            if (string.IsNullOrEmpty(infoRequest.OldPassword))
                            {
                                return CreateValidationProblem("OldPasswordRequired",
                                    "The old password is required to set a new password. If the old password is forgotten, use /resetPassword.");
                            }

                            var changePasswordResult = await userManager.ChangePasswordAsync(user, infoRequest.OldPassword, infoRequest.NewPassword);
                            if (!changePasswordResult.Succeeded)
                            {
                                return CreateValidationProblem(changePasswordResult);
                            }
                        }

                        if (!string.IsNullOrEmpty(infoRequest.NewEmail))
                        {
                            var email = await userManager.GetEmailAsync(user);

                            if (email != infoRequest.NewEmail)
                            {
                                await SendConfirmationEmailAsync(user, userManager, context, infoRequest.NewEmail, isChange: true);
                            }
                        }

                        return TypedResults.Ok(await CreateInfoResponseAsync(user, userManager));
                    });
                }
            }

            async Task SendConfirmationEmailAsync(TUser user, UserManager<TUser> userManager, HttpContext context, string email, bool isChange = false)
            {
                if (confirmEmailEndpointName is null)
                {
                    throw new NotSupportedException("No email confirmation endpoint was registered!");
                }

                var code = isChange
                    ? await userManager.GenerateChangeEmailTokenAsync(user, email)
                    : await userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var userId = await userManager.GetUserIdAsync(user);
                var routeValues = new RouteValueDictionary()
                {
                    ["userId"] = userId,
                    ["code"] = code,
                };

                if (isChange)
                {
                    // This is validated by the /confirmEmail endpoint on change.
                    routeValues.Add("changedEmail", email);
                }

                var confirmEmailUrl = linkGenerator.GetUriByName(context, confirmEmailEndpointName, routeValues)
                    ?? throw new NotSupportedException($"Could not find endpoint named '{confirmEmailEndpointName}'.");

                await emailSender.SendConfirmationLinkAsync(user, email, HtmlEncoder.Default.Encode(confirmEmailUrl));
            }

            return new IdentityEndpointsConventionBuilder(routeGroup);
        }

        private static ValidationProblem CreateValidationProblem(string errorCode, string errorDescription) =>
            TypedResults.ValidationProblem(new Dictionary<string, string[]> {
            { errorCode, [errorDescription] }
            });

        private static ValidationProblem CreateValidationProblem(IdentityResult result)
        {

            Debug.Assert(!result.Succeeded);
            var errorDictionary = new Dictionary<string, string[]>(1);

            foreach (var error in result.Errors)
            {
                string[] newDescriptions;

                if (errorDictionary.TryGetValue(error.Code, out var descriptions))
                {
                    newDescriptions = new string[descriptions.Length + 1];
                    Array.Copy(descriptions, newDescriptions, descriptions.Length);
                    newDescriptions[descriptions.Length] = error.Description;
                }
                else
                {
                    newDescriptions = [error.Description];
                }

                errorDictionary[error.Code] = newDescriptions;
            }

            return TypedResults.ValidationProblem(errorDictionary);
        }

        private static async Task<InfoResponse> CreateInfoResponseAsync<TUser>(TUser user, UserManager<TUser> userManager)
            where TUser : class
        {
            return new()
            {
                Email = await userManager.GetEmailAsync(user) ?? throw new NotSupportedException("Users must have an email."),
                IsEmailConfirmed = await userManager.IsEmailConfirmedAsync(user),
            };
        }

        private static async Task<Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>> SignInAsync<TUser>
            (SignInManager<TUser> signInManager, LoginRequest login, IServiceProvider sp)
            where TUser : class
        {
            signInManager.AuthenticationScheme = IdentityConstants.BearerScheme;
            var user = await signInManager.UserManager.FindByEmailAsync(login.Email);
            if (user == null)
            {
                return TypedResults.Problem("Invalid UserName or Password", statusCode: StatusCodes.Status401Unauthorized);
            }
            var result = await signInManager.PasswordSignInAsync(login.Email, login.Password, false, lockoutOnFailure: true);

            if(!result.Succeeded)
            {
                if (!result.Succeeded)
                {
                    return TypedResults.Problem(result.ToString(), statusCode: StatusCodes.Status401Unauthorized);
                }

            }
            
            return TypedResults.Empty;
        }

        private sealed class IdentityEndpointsConventionBuilder(RouteGroupBuilder inner) : IEndpointConventionBuilder
        {
            private IEndpointConventionBuilder InnerAsConventionBuilder => inner;

            public void Add(Action<EndpointBuilder> convention) => InnerAsConventionBuilder.Add(convention);
            public void Finally(Action<EndpointBuilder> finallyConvention) => InnerAsConventionBuilder.Finally(finallyConvention);
        }

        [AttributeUsage(AttributeTargets.Parameter)]
        private sealed class FromBodyAttribute : Attribute, IFromBodyMetadata
        {
        }

        [AttributeUsage(AttributeTargets.Parameter)]
        private sealed class FromServicesAttribute : Attribute, IFromServiceMetadata
        {
        }

        [AttributeUsage(AttributeTargets.Parameter)]
        private sealed class FromQueryAttribute : Attribute, IFromQueryMetadata
        {
            public string? Name => null;
        }
    }
}