using CleanArchitecture.Application.Vehiculos;
using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers.Vehiculos
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class VehiculosController : ControllerBase
    {
        private readonly ISender _sender;

        public VehiculosController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> SearchVehiculos(
            DateOnly desde,
            DateOnly hasta,
            CancellationToken cancellationToken
        )
        {
            var query = new SearchAvaibleByDateRangeFeature.Query(desde, hasta);

            var result = await _sender.Send(query, cancellationToken);

            return Ok(result.Value);
        }
    }
}
