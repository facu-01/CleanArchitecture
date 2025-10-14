using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Alquileres;
public interface IAlquilerRepository : IGenericRepository<Alquiler>
{
    Task<bool> IsOverlapping(DateRange periodo, Guid vehiculoId, AlquilerStatus[] statuses, CancellationToken cancellationToken = default);
}
