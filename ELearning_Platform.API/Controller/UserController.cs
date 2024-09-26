﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using ELearning_Platform.Infrastructure.Services.UserServices.Queries;
using ELearning_Platform.Infrastructure.Services.UserServices.Queries.Informations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using ELearning_Platform.Infrastructure.Services.UserServices.Queries.AllUserInformations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
namespace ELearning_Platform.API.Controller
{
    [ApiController]
    [Route("api/user")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController(IMediator mediator):ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        
        [HttpGet("informations")]
        public async Task<IActionResult> GetInfromationsAboutCurrentUser(CancellationToken token)
        {
            var result = await _mediator.Send(request: new GetUserInformationsAsyncQuery(), cancellationToken: token);
            return Ok(result);
        }
   
        [HttpGet("informations/all")]
        public async Task<IActionResult> GetInfromationsAboutAllUsers(CancellationToken token)
            => Ok(await _mediator.Send(request: new GetInfromationsAboutAllUsersAsyncQuery(), cancellationToken: token));
    }
}
