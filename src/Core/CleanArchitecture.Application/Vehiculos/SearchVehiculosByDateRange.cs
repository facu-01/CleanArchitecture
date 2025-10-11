using System;
using CleanArchitecture.Application.Abstractions.DataAccess;
using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Vehiculos;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Vehiculos;

public static class SearchVehiculosByDateRange
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
        private readonly IApplicationDbContext _applicationDbContext;
        private static readonly int[] ActiveAlquilerStatuses =
        [
            (int)AlquilerStatus.Confirmado,
            (int)AlquilerStatus.Reservado,
            (int)AlquilerStatus.Completado
        ];

        public Handler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Result<List<VehiculoResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            if (request.Desde > request.Hasta)
            {
                return Result.Success(new List<VehiculoResponse>());
            }

            var vehiculos = _applicationDbContext.Vehiculos;
            var alquileres = _applicationDbContext.Alquileres;

            var vehiculosDisp = await vehiculos.GroupJoin(
                alquileres.Where(a =>
                    ActiveAlquilerStatuses.Contains((int)a.Status) &&
                    a.Periodo.Inicio <= request.Hasta &&
                    a.Periodo.Fin >= request.Desde
                ),
                vehiculo => vehiculo.Id,
                alquiler => alquiler.VehiculoId,
                (vehiculo, alquileres) => new { Vehiculo = vehiculo, Alquileres = alquileres })
                .SelectMany(
                    x => x.Alquileres.DefaultIfEmpty(),
                    (joined, alquiler) => new { joined.Vehiculo, Alquiler = alquiler }
                )
                .Where(joined => joined.Alquiler == null)
                .Select(joined => joined.Vehiculo)
                .ToListAsync(cancellationToken);

            var vehiculosDispResp = vehiculosDisp.Select(v => new VehiculoResponse
            {
                Id = v.Id,
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
