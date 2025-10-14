using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Alquileres;

namespace CleanArchitecture.Domain.Vehiculos;
public interface IVehiculoRepository : IGenericRepository<Vehiculo,VehiculoId>
{
    Task<List<Vehiculo>> VehiculosDisponibles(DateRange dateRange, AlquilerStatus[] alquilerStatuses, CancellationToken cancellationToken);
}
