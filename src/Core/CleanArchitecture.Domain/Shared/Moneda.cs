namespace CleanArchitecture.Domain.Shared;

public sealed record Moneda(decimal Monto, TipoMoneda TipoMoneda)
{

    public static Moneda operator +(Moneda pri, Moneda snd)
    {
        if (pri.TipoMoneda != snd.TipoMoneda)
        {
            throw new InvalidOperationException("El tipo de moneda debe ser el mismo");
        }
        return new Moneda(pri.Monto + snd.Monto, pri.TipoMoneda);
    }

    public static Moneda Zero(TipoMoneda tipoMoneda) => new(0, tipoMoneda);

    public bool IsZero => this == Zero(TipoMoneda);

}
