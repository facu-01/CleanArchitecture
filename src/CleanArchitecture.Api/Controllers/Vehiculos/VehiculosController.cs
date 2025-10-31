using CleanArchitecture.Application.Vehiculos;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Permissions;
using CleanArchitecture.Domain.Vehiculos;
using CleanArchitecture.Infraestructure.Authentication;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers.Vehiculos
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiculosController : ControllerBase
    {
        private readonly ISender _sender;

        public VehiculosController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [HasPermission(PermissionEnum.ReadUser)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SearchAvaibleByDateRangeFeature.VehiculoResponse>))]
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

        [HttpGet("paginated")]
        [HasPermission(PermissionEnum.ReadUser)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResult<Vehiculo, VehiculoId>))]
        public async Task<IActionResult> GetVehiculosPaginated(
            [FromQuery] GetVehiculosPaginated.Query query,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(query, cancellationToken);
            return Ok(result.Value);
        }

    }
}
