using System;
using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Alquileres;

public static class AlquilerErrors
{

    public static Error NotReserved =
    Error.MakeError<Alquiler>(
        nameof(NotReserved),
        "El alquiler no esta reservado"
    );

    public static Error NotConfirmado =
    Error.MakeError<Alquiler>(
        nameof(NotConfirmado),
        "El alquiler no esta confirmado"
    );

    public static Error AlreadyStarted =
    Error.MakeError<Alquiler>(
        nameof(AlreadyStarted),
        "El alquiler ya ha comenzado"
    );

}
