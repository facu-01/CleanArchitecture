using System;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Shared;

namespace CleanArchitecture.Domain.Vehiculos;

public sealed class Vehiculo : Entity
{
    public Vehiculo(
        Guid id,
        Modelo modelo,
        Vin vin,
        Moneda precio,
        Moneda mantenimiento,
        List<Accesorio> accesorios,
        DateTime? fechaUltimoAlquiler
        ) : base(id)
    {

        Modelo = modelo;
        Vin = vin;
        Precio = precio;
        Mantenimiento = mantenimiento;
        Accesorios = accesorios;
        FechaUltimoAlquiler = fechaUltimoAlquiler;
    }

    public Modelo Modelo { get; private set; }

    public Vin Vin { get; private set; }
    public Moneda Precio { get; private set; }
    public Moneda Mantenimiento { get; private set; }
    public List<Accesorio> Accesorios { get; private set; }
    public DateTime? FechaUltimoAlquiler { get; private set; }


    internal void AlquilarFecha(DateTime FechaAlquiler)
    {
        FechaUltimoAlquiler = FechaAlquiler;
    }


}
