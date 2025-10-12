using CleanArchitecture.Application.Alquileres;
using CleanArchitecture.Domain.Alquileres;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers.Alquileres
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlquileresController : ControllerBase
    {
        private readonly ISender _sender;

        public AlquileresController(ISender sender)
        {
            _sender = sender;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetAlquilerById.Query(id);

            var result = await _sender.Send(query, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }


        public sealed record AlquilerReservaRequest(
            Guid VehiculoId,
            Guid UserId,
            DateOnly Desde,
            DateOnly Hasta
        );

        [HttpPost]
        public async Task<IActionResult> ReservarAlquiler(
            Guid id,
            AlquilerReservaRequest request,
            CancellationToken cancellationToken
        )
        {
            var command = new ReservarAlquiler.Command(
                        request.VehiculoId,
                        request.UserId,
                        request.Desde,
                        request.Hasta
            );

            var result = await _sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return result.Error.StatusCode switch
                {
                    System.Net.HttpStatusCode.NotFound => NotFound(result.Error),
                    _ => BadRequest(result.Error)
                };
            }

            return CreatedAtAction(nameof(GetAlquilerById), new { id = result.Value });
        }


    }


}
