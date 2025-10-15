using System.Net;

namespace CleanArchitecture.Domain.Abstractions;

public record Error(string Detail, string Message, HttpStatusCode? StatusCode = null)
{

    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "Un valor Null fue ingresado");

    public static Error MakeError<T>(string errorCode, string message, HttpStatusCode? statusCode = null)
    {
        return new Error(
            $"{typeof(T).Name}.{errorCode}",
            message,
            statusCode
        );
    }

}