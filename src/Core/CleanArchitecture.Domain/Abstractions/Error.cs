using System;

namespace CleanArchitecture.Domain.Abstractions;

public record Error(string Code, string Message)
{

    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "Un valor Null fue ingresado");

    public static Error MakeError<T>(string errorCode, string message)
    {
        return new Error(
            $"{typeof(T).Name}.{errorCode}",
            message
        );
    }

}