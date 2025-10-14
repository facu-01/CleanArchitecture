using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Vehiculos;

using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infraestructure.DataAccess.Repositories;
internal sealed class VehiculoRepository : GenericRepository<Vehiculo,VehiculoId>, IVehiculoRepository
{
    public VehiculoRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<Vehiculo>> VehiculosDisponibles(DateRange dateRange, AlquilerStatus[] alquilerStatuses, CancellationToken cancellationToken)
    {
        var vehiculos = _dbContext.Vehiculos;
        var alquileres = _dbContext.Alquileres;

        var vehiculosDisp = await vehiculos.AsNoTracking()
        .GroupJoin(
            alquileres.Where(a =>
                alquilerStatuses.Contains(a.Status) &&
                a.Periodo.Inicio <= dateRange.Fin &&
                a.Periodo.Fin >= dateRange.Inicio
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

        return vehiculosDisp;
    }
}
