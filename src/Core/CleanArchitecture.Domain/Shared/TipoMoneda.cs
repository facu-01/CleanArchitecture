using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Shared;

public sealed record TipoMoneda
{

    public string Codigo { get; private set; }

    public static Error InvalidCode(string codigo) => new(
        nameof(InvalidCode),
        nameof(TipoMoneda),
        $"El codigo {codigo} no es valido");

    public static readonly TipoMoneda Usd = new("USD");
    public static readonly TipoMoneda Eur = new("EUR");

    private TipoMoneda(string codigo) => Codigo = codigo;


    public static Result<TipoMoneda> FromCodigo(string codigo) =>
    codigo switch
    {
        "USD" => Result.Success(Usd),
        "EUR" => Result.Success(Eur),
        _ => Result.Failure<TipoMoneda>(InvalidCode(codigo))
    };


}
