using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Users;

public static class UserErrors
{
    public static Error NotFound(UserId id) => EntityErrors<User>.NotFound(id);

    public static Error NotFound(Email email) => Error.MakeError<User>(
        nameof(NotFound),
        $"Usuario no encontrado con email {email.Value}"
    );

    public static Error EmailYaEnUso(Email email) =>
        Error.MakeError<User>(
        nameof(EmailYaEnUso),
        $"El email {email} ya se encuentra en uso"
    );


    public static Error InvalidCredentials() =>
        Error.MakeError<User>(
        nameof(InvalidCredentials),
        "Las credenciales indicadas son inválidas"
    );

}
