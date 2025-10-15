using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Users;

public static class UserErrors
{
    public static Error NotFound => EntityErrors<User>.NotFound();
    public static Error EmailYaEnUso =>
        Error.MakeError<User>(
        nameof(EmailYaEnUso),
        $"El email ya se encuentra en uso"
    );


    public static Error InvalidCredentials() =>
        Error.MakeError<User>(
        nameof(InvalidCredentials),
        "Las credenciales indicadas son inválidas"
    );

}
