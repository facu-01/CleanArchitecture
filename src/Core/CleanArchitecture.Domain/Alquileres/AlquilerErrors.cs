using System;
using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Alquileres;

public static class AlquilerErrors
{

    public static readonly Error Overlap =
    Error.MakeError<Alquiler>(
        nameof(Overlap),
        "El Alquiler esta siendo tomado por 2 o mas clientes al mismo tiempo en la misma fecha"
    );

    public static readonly Error NotReserved =
    Error.MakeError<Alquiler>(
        nameof(NotReserved),
        "El alquiler no esta reservado"
    );

    public static readonly Error NotConfirmado =
    Error.MakeError<Alquiler>(
        nameof(NotConfirmado),
        "El alquiler no esta confirmado"
    );

    public static readonly Error AlreadyStarted =
    Error.MakeError<Alquiler>(
        nameof(AlreadyStarted),
        "El alquiler ya ha comenzado"
    );

}
