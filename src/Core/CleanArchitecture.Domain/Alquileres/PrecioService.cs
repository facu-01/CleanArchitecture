using System;
using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Domain.Vehiculos;

namespace CleanArchitecture.Domain.Alquileres;

public class PrecioService
{

    public PrecioDetalle CalcularPrecio
    (
        Vehiculo vehiculo,
        DateRange dateRange
    )
    {
        var tipoMoneda = vehiculo.Precio.TipoMoneda;

        var precioPeriodo = new Moneda(
            dateRange.CantidadDias * vehiculo.Precio.Monto,
            tipoMoneda
        );

        var porcentageAccesorios =
        vehiculo.Accesorios.Sum(
            acc => acc switch
            {
                Accesorio.AppleCar or Accesorio.AndroidCar => 0.05m,
                Accesorio.AireAcondicionado => 0.01m,
                Accesorio.Mapas => 0.01m,
                _ => 0
            }
        );

        var precioAccesorios = new Moneda(
            precioPeriodo.Monto * porcentageAccesorios,
            tipoMoneda
        );

        return new PrecioDetalle(
            precioPeriodo,
            vehiculo.Mantenimiento,
            precioAccesorios,
            precioPeriodo + vehiculo.Mantenimiento + precioAccesorios
        );


    }

}
