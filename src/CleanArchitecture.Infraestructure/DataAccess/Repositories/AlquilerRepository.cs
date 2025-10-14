using CleanArchitecture.Domain.Alquileres;

using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infraestructure.DataAccess.Repositories;
internal sealed class AlquilerRepository : GenericRepository<Alquiler>, IAlquilerRepository
{

    public AlquilerRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> IsOverlapping(
        DateRange periodo,
        Guid vehiculoId,
        AlquilerStatus[] statuses,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Alquileres.AnyAsync(
                   a =>
                   a.VehiculoId == vehiculoId &&
                   a.Periodo.Inicio <= periodo.Fin &&
                   a.Periodo.Fin >= periodo.Inicio &&
                   statuses.Contains(a.Status)
        );
    }


}
