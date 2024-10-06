﻿using ELearning_Platform.Application.Services.AccountServices.Command.RefreshToken;
using ELearning_Platform.Application.Services.AccountServices.Command.SignIn;
using ELearning_Platform.Infrastructure.Services.AccountServices.Command.Register;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ELearning_Platform.API.Controller
{
    [ApiController]
    [Route("api/account")]
    public class AccountController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("registration")]
        public async Task<IActionResult> RegisterAsync(RegisterAccountAsyncCommand register, CancellationToken token)
        {
            await _mediator.Send(request: register, cancellationToken: token);

            return Created(string.Empty, new { Email = register.AddressEmail, Message = $"Your account have been created: {register.FirstName} {register.Surname}" });
        }
        [HttpPost("signin")]
        public async Task<IActionResult> SignInAsync(SignInAsyncCommand signInAsyncCommand, CancellationToken token)
           => Ok(await _mediator.Send(request: signInAsyncCommand, cancellationToken: token));
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokenAsync(CancellationToken token)
            => Ok(await _mediator.Send(new RefreshTokenAsyncCommand(), cancellationToken: token));
    }
}
