using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Alquileres;

namespace CleanArchitecture.Application.Alquileres;

public static class GetByIdFeature
{

    public sealed class AlquilerResponse
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public Guid VehiculoId { get; init; }
        public int Status { get; init; }
        public decimal PrecioAlquiler { get; init; }
        public string TipoMonedaAlquiler { get; init; } = string.Empty;
        public decimal PrecioMantenimiento { get; init; }
        public string TipoMonedaMantenimiento { get; init; } = string.Empty;
        public decimal PrecioAccesorios { get; init; }
        public string TipoMonedaAccesorios { get; init; } = string.Empty;
        public decimal PrecioTotal { get; init; }
        public string TipoMonedaTotal { get; init; } = string.Empty;
        public DateOnly FechaInicio { get; init; }
        public DateOnly FechaFinal { get; init; }
        public DateTime FechaCreacion { get; init; }
    }

    public record Query(Guid Id) : IQuery<AlquilerResponse>;


    internal sealed class Handler : IQueryHandler<Query, AlquilerResponse>
    {

        private readonly IAlquilerRepository _alquilerRepository;

        public Handler(IAlquilerRepository alquilerRepository)
        {
            _alquilerRepository = alquilerRepository;
        }

        public async Task<Result<AlquilerResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var alquilerId = new AlquilerId(request.Id);
            var alquiler = await _alquilerRepository.GetByIdAsync(alquilerId, cancellationToken);

            if (alquiler is null)
            {
                return Result.Failure<AlquilerResponse>(AlquilerErrors.NotFound(alquilerId));
            }

            var alquilerResponse = new AlquilerResponse
            {
                FechaCreacion = alquiler.FechaCreacion,
                FechaFinal = alquiler.Periodo.Fin,
                FechaInicio = alquiler.Periodo.Inicio,
                Id = alquiler.Id.Value,
                PrecioAccesorios = alquiler.PrecioAccesorios.Monto,
                TipoMonedaAccesorios = alquiler.PrecioAccesorios.TipoMoneda.Codigo,
                PrecioAlquiler = alquiler.PrecioPorPeriodo.Monto,
                TipoMonedaAlquiler = alquiler.PrecioPorPeriodo.TipoMoneda.Codigo,
                PrecioMantenimiento = alquiler.PrecioMantenimiento.Monto,
                TipoMonedaMantenimiento = alquiler.PrecioMantenimiento.TipoMoneda.Codigo,
                PrecioTotal = alquiler.PrecioTotal.Monto,
                TipoMonedaTotal = alquiler.PrecioTotal.TipoMoneda.Codigo,
                Status = (int)alquiler.Status,
                UserId = alquiler.UserId.Value,
                VehiculoId = alquiler.VehiculoId.Value,
            };

            return Result.Success(alquilerResponse);
        }
    }

}
