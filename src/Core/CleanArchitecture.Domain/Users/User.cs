using System;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Users.Events;

namespace CleanArchitecture.Domain.Users;

public sealed class User : Entity<UserId>
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private User() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    private User(
        UserId id,
        Nombre nombre,
        Apellido apellido,
        Email email
        ) : base(id)
    {
        Nombre = nombre;
        Apellido = apellido;
        Email = email;
    }

    public Nombre Nombre { get; private set; }
    public Apellido Apellido { get; private set; }
    public Email Email { get; private set; }

    public static User Registrar(
        Nombre nombre,
        Apellido apellido,
        Email email
    )
    {
        var user = new User(
            UserId.New(),
            nombre,
            apellido,
            email
        );

        user.RaiseDomainEvent(new UserRegisteredDomainEvent(user.Id));

        return user;
    }

}
