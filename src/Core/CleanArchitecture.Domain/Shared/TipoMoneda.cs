namespace CleanArchitecture.Domain.Shared;

public sealed record TipoMoneda
{

    public string Codigo { get; private set; }

    public static readonly TipoMoneda Usd = new("USD");
    public static readonly TipoMoneda Eur = new("EUR");

    private TipoMoneda(string codigo) => Codigo = codigo;


    public static TipoMoneda FromCodigo(string codigo) =>
    codigo switch
    {
        "USD" => new(codigo),
        "EUR" => new(codigo),
        _ => throw new ArgumentOutOfRangeException(codigo, $"El codigo de moneda no es válido {codigo}")
    };


}
