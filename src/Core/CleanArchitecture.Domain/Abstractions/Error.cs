namespace CleanArchitecture.Domain.Abstractions;

public record Error(string Code, string Detail, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error", "NullValue", "Un valor Null fue ingresado");
}