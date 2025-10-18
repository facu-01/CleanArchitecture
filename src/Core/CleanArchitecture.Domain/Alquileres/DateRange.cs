using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Alquileres;

public sealed record class DateRange
{
    private DateRange() { }

    public static Error InvalidDateRange => new(
        nameof(InvalidDateRange),
        nameof(DateRange),
        "El rango de fechas especificado es invalido"
    );

    public DateOnly Inicio { get; init; }
    public DateOnly Fin { get; init; }

    public int CantidadDias => Fin.DayNumber - Inicio.DayNumber;

    public static Result<DateRange> Create(DateOnly inicio, DateOnly fin)
    {
        if (inicio > fin)
        {
            return Result.Failure<DateRange>(InvalidDateRange);
        }

        return Result.Success(
            new DateRange
            {
                Inicio = inicio,
                Fin = fin
            }
        );
    }


}
