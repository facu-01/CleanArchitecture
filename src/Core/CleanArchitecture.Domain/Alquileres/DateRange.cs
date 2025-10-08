namespace CleanArchitecture.Domain.Alquileres;

public sealed record class DateRange
{
    private DateRange() {}

    public DateOnly Inicio { get; init; }
    public DateOnly Fin { get; init; }

    public static DateRange Create(DateOnly inicio, DateOnly fin)
    {
        if (inicio > fin)
        {
            throw new ApplicationException("La fecha final debe ser mayor a la fecha inicio");
            
        }
        return new DateRange
        {
            Inicio = inicio,
            Fin = fin
        };
    }


}
