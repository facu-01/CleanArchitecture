using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Vehiculos;

namespace CleanArchitecture.Domain.Alquileres;
public interface IAlquilerRepository : IGenericRepository<Alquiler,AlquilerId>
{
    Task<bool> IsOverlapping(DateRange periodo, VehiculoId vehiculoId, AlquilerStatus[] statuses, CancellationToken cancellationToken = default);
}
