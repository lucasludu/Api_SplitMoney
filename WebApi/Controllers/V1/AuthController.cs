using Application.Features._auth.Commands.LoginCommands;
using Application.Features._auth.Commands.RegisterUserCommands;
using Application.Features._auth.Commands.RefreshTokenCommands;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Application.Features._auth.DTOs.Request;
using Application.Features._auth.Commands.LogoutCommands;

namespace WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    public class AuthController : BaseApiController
    {
        /// <summary>
        /// Login a user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await Mediator.Send(new LoginCommand(request));

            return (!result.Succeeded)
                ? Unauthorized(result)
                : Ok(result);
        }

        /// <summary>
        /// Register a user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var result = await Mediator.Send(new RegisterUserCommand(request));

            return (!result.Succeeded)
                ? BadRequest(result)
                : Ok(result);
        }

        /// <summary>
        /// Refresh a JWT token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var result = await Mediator.Send(new RefreshTokenCommand(request));

            return (!result.Succeeded)
                ? Unauthorized(result)
                : Ok(result);
        }

        /// <summary>
        /// Logout a user (revoke refresh token)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request)
        {
            var result = await Mediator.Send(new LogoutCommand(request.RefreshToken));

            return (!result.Succeeded)
                ? BadRequest(result)
                : Ok(result);
        }
    }
}
