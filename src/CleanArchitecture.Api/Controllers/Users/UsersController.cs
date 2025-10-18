using CleanArchitecture.Api.Extensions;
using CleanArchitecture.Application.Users;
using CleanArchitecture.Domain.Users;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
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
                    nameof(UserErrors.NotFound) => NotFound(result.Error.ToProblemDetails()),
                    nameof(UserErrors.InvalidCredentials) => Unauthorized(result.Error.ToProblemDetails()),
                    _ => BadRequest(result.Error.ToProblemDetails())
                };
            }

            return Content(result.Value);
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
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ProblemDetails))]
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
                    nameof(UserErrors.EmailYaEnUso) => Conflict(result.Error.ToProblemDetails()),
                    _ => BadRequest(result.Error.ToProblemDetails())
                };
            }

            return Ok();
        }

    }
}
