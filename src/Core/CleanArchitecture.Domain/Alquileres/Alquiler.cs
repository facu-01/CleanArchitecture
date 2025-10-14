using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Alquileres.Events;
using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Domain.Vehiculos;

namespace CleanArchitecture.Domain.Alquileres;

public class Alquiler : Entity
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Alquiler() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    private Alquiler(
        Guid id,
        Guid vehiculoId,
        Guid userId,
        DateRange periodo,
        Moneda precioPorPeriodo,
        Moneda precioMantenimiento,
        Moneda precioAccesorios,
        Moneda precioTotal,
        AlquilerStatus status,
        DateTime fechaCreacion
        ) : base(id)
    {
        VehiculoId = vehiculoId;
        UserId = userId;
        Periodo = periodo;
        PrecioPorPeriodo = precioPorPeriodo;
        PrecioMantenimiento = precioMantenimiento;
        PrecioAccesorios = precioAccesorios;
        PrecioTotal = precioTotal;
        Status = status;
        FechaCreacion = fechaCreacion;
    }

    public Guid VehiculoId { get; private set; }
    public Guid UserId { get; private set; }
    public DateRange Periodo { get; private set; }
    public Moneda PrecioPorPeriodo { get; private set; }
    public Moneda PrecioMantenimiento { get; private set; }
    public Moneda PrecioAccesorios { get; private set; }
    public Moneda PrecioTotal { get; private set; }
    public AlquilerStatus Status { get; private set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaConfirmacion { get; private set; }
    public DateTime? FechaDenegacion { get; private set; }
    public DateTime? FechaCompletado { get; private set; }
    public DateTime? FechaCancelacion { get; private set; }

    public static Alquiler Reservar
    (
        Vehiculo vehiculo,
        Guid userId,
        DateRange periodo,
        DateTime fechaCreacion,
        PrecioService precioService
    )
    {
        var precioDetalle = precioService.CalcularPrecio(
            vehiculo,
            periodo
        );

        var alquiler = new Alquiler(
            Guid.NewGuid(),
            vehiculo.Id,
            userId,
            periodo,
            precioDetalle.PrecioPorPeriodo,
            precioDetalle.PrecioMantenimiento,
            precioDetalle.PrecioAccesorios,
            precioDetalle.PrecioTotal,
            AlquilerStatus.Reservado,
            fechaCreacion
        );

        alquiler.RaiseDomainEvent(new AlquilerReservadoDomainEvent(alquiler.Id));

        vehiculo.AlquilarFecha(fechaCreacion);
        return alquiler;
    }

    public Result Confirmar(DateTime fecha)
    {
        if (Status != AlquilerStatus.Reservado)
        {
            return Result.Failure(AlquilerErrors.NotReserved);
        }

        FechaConfirmacion = fecha;
        Status = AlquilerStatus.Confirmado;

        RaiseDomainEvent(new AlquilerConfirmadoDomainEvent(Id));

        return Result.Success();
    }

    public Result Rechazar(DateTime fecha)
    {
        if (Status != AlquilerStatus.Reservado)
        {
            return Result.Failure(AlquilerErrors.NotReserved);
        }

        FechaDenegacion = fecha;
        Status = AlquilerStatus.Rechazado;

        RaiseDomainEvent(new AlquilerRechazadoDomainEvent(Id));

        return Result.Success();
    }

    public Result Cancelar(DateTime fecha)
    {
        if (Status != AlquilerStatus.Confirmado)
        {
            return Result.Failure(AlquilerErrors.NotConfirmado);
        }

        var currentDate = DateOnly.FromDateTime(fecha);

        if (currentDate >= Periodo.Inicio)
        {
            return Result.Failure(AlquilerErrors.AlreadyStarted);
        }


        FechaCancelacion = fecha;
        Status = AlquilerStatus.Cancelado;

        RaiseDomainEvent(new AlquilerCanceladoDomainEvent(Id));

        return Result.Success();
    }

    public Result Completar(DateTime fecha)
    {
        if (Status != AlquilerStatus.Confirmado)
        {
            return Result.Failure(AlquilerErrors.NotConfirmado);
        }

        FechaCompletado = fecha;
        Status = AlquilerStatus.Completado;

        RaiseDomainEvent(new AlquilerCompletadoDomainEvent(Id));

        return Result.Success();
    }


}
