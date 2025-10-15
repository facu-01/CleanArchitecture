using CleanArchitecture.Application.Users;

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
        [HttpPost]
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
                return result.Error.StatusCode switch
                {
                    System.Net.HttpStatusCode.NotFound => NotFound(result.Error),
                    System.Net.HttpStatusCode.Unauthorized => Unauthorized(result.Error),
                    _ => BadRequest(result.Error)
                };
            }

            return Ok(result.Value);
        }

    }
}
