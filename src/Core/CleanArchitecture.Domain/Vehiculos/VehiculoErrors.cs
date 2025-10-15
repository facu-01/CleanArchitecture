using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Vehiculos;
public static class VehiculoErrors
{
    public static Error NotFound => EntityErrors<Vehiculo>.NotFound();
}
