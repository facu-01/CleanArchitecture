using CleanArchitecture.Domain.Shared;

namespace CleanArchitecture.Domain.Alquileres;

public record class PrecioDetalle(
    Moneda PrecioPorPeriodo,
    Moneda PrecioMantenimiento,
    Moneda PrecioAccesorios,
    Moneda PrecioTotal
);
