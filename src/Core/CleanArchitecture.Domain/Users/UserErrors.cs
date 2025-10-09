using System;
using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Users;

public static class UserErrors
{

    public static Error EmailYaEnUso(Email email) =>
        Error.MakeError<User>(
        nameof(EmailYaEnUso),
        $"El email {email.Value} ya se encuentra en uso"
    );

}
