using System;
using CleanArchitecture.Application.Abstractions.DataAccess;
using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Alquileres;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Alquileres;

public static class GetAlquilerById
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

        private readonly IApplicationDbContext _applicationDbContext;

        public Handler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Result<AlquilerResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var alquiler = await _applicationDbContext.Alquileres
                            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

            if (alquiler is null)
            {
                return Result.Failure<AlquilerResponse>(EntityErrors.NotFound<Alquiler>(request.Id));
            }

            var alquilerResponse = new AlquilerResponse
            {
                FechaCreacion = alquiler.FechaCreacion,
                FechaFinal = alquiler.Periodo.Fin,
                FechaInicio = alquiler.Periodo.Inicio,
                Id = alquiler.Id,
                PrecioAccesorios = alquiler.PrecioAccesorios.Monto,
                TipoMonedaAccesorios = alquiler.PrecioAccesorios.TipoMoneda.Codigo,
                PrecioAlquiler = alquiler.PrecioPorPeriodo.Monto,
                TipoMonedaAlquiler = alquiler.PrecioPorPeriodo.TipoMoneda.Codigo,
                PrecioMantenimiento = alquiler.PrecioMantenimiento.Monto,
                TipoMonedaMantenimiento = alquiler.PrecioMantenimiento.TipoMoneda.Codigo,
                PrecioTotal = alquiler.PrecioTotal.Monto,
                TipoMonedaTotal = alquiler.PrecioTotal.TipoMoneda.Codigo,
                Status = ((int)alquiler.Status),
                UserId = alquiler.UserId,
                VehiculoId = alquiler.VehiculoId,
            };

            return Result.Success(alquilerResponse);
        }
    }

}
