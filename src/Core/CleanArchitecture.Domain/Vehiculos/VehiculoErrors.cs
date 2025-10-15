using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Vehiculos;
public static class VehiculoErrors
{
    public static Error NotFound(VehiculoId id) => EntityErrors<Vehiculo>.NotFound(id);
}
