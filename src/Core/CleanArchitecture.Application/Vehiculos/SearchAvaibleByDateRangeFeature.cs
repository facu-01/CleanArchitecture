using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Vehiculos;

namespace CleanArchitecture.Application.Vehiculos;

public static class SearchAvaibleByDateRangeFeature
{

    public sealed class VehiculoResponse
    {
        public Guid Id { get; init; }
        public string Modelo { get; init; } = string.Empty;
        public string Vin { get; init; } = string.Empty;
        public decimal Precio { get; init; }
        public string TipoMoneda { get; init; } = string.Empty;
        public DireccionResponse? Direccion { get; init; } = null;

        public sealed class DireccionResponse
        {
            public string Pais { get; init; } = string.Empty;
            public string Departamento { get; init; } = string.Empty;
            public string Provincia { get; init; } = string.Empty;
            public string Calle { get; init; } = string.Empty;
        }

    }

    public sealed record Query(DateOnly Desde, DateOnly Hasta) : IQuery<List<VehiculoResponse>>;


    internal sealed class Handler : IQueryHandler<Query, List<VehiculoResponse>>
    {
        private readonly IVehiculoRepository _vehiculoRepository;

        public Handler(IVehiculoRepository vehiculoRepository)
        {
            _vehiculoRepository = vehiculoRepository;
        }

        private static readonly AlquilerStatus[] _activeAlquilerStatuses =
        [
            AlquilerStatus.Confirmado,
            AlquilerStatus.Reservado,
            AlquilerStatus.Completado
        ];


        public async Task<Result<List<VehiculoResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {

            var dateRange = DateRange.Create(request.Desde, request.Hasta);

            if (dateRange.IsFailure)
            {
                return Result.Success(new List<VehiculoResponse>());
            }

            var vehiculosDisp = await _vehiculoRepository.VehiculosDisponibles(
                dateRange.Value,
                _activeAlquilerStatuses,
                cancellationToken
            );

            var vehiculosDispResp = vehiculosDisp.Select(v => new VehiculoResponse
            {
                Id = v.Id.Value,
                Modelo = v.Modelo.Value,
                Vin = v.Vin.Value,
                Precio = v.Precio.Monto,
                TipoMoneda = v.Precio.TipoMoneda.Codigo,
                Direccion = new()
                {
                    Calle = v.Direccion.Calle,
                    Departamento = v.Direccion.Departamento,
                    Pais = v.Direccion.Pais,
                    Provincia = v.Direccion.Provincia
                }
            }).ToList();

            return Result.Success(vehiculosDispResp);
        }
    }




}
