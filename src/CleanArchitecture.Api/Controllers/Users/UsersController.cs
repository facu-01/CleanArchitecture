using CleanArchitecture.Application.Users;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Users;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ISender _sender;

        public UsersController(ISender sender)
        {
            _sender = sender;
        }


        public sealed record LoginRequest(
            string Email,
            string Password
        );

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest request,
            CancellationToken cancellationToken
        )
        {
            var command = new LoginFeature.Command(
                    request.Email,
                    request.Password
            );

            var result = await _sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return result.Error.Code switch
                {
                    nameof(UserErrors.NotFound) => NotFound(result.Error),
                    nameof(UserErrors.InvalidCredentials) => Unauthorized(result.Error),
                    _ => BadRequest(result.Error)
                };
            }

            return Ok(result.Value);
        }


        public sealed record RegisterRequest(
            string Apellido,
            string Nombre,
            string Email,
            string Password
        );

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Register(
            [FromBody] RegisterRequest request,
            CancellationToken cancellationToken
        )
        {
            var command = new RegisterFeature.Command(
                    request.Nombre,
                    request.Apellido,
                    request.Email,
                    request.Password
            );

            var result = await _sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return result.Error.Code switch
                {
                    nameof(UserErrors.EmailYaEnUso) => Conflict(result.Error),
                    _ => BadRequest(result.Error)
                };
            }

            return Ok();
        }

    }
}
