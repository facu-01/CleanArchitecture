using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Users;

public sealed class UserErrors : EntityErrors<User>
{

    public static Error NotFound(Email email) => MakeError(
        nameof(NotFound),
        $"Usuario no encontrado con email {email.Value}"
    );

    public static Error EmailYaEnUso(Email email) =>
        MakeError(
        nameof(EmailYaEnUso),
        $"El email {email} ya se encuentra en uso"
    );


    public static Error InvalidCredentials() =>
        MakeError(
        nameof(InvalidCredentials),
        "Las credenciales indicadas son inválidas"
    );

}
