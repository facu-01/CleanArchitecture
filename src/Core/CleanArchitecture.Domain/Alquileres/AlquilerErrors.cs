using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Alquileres;

public sealed class AlquilerErrors : EntityErrors<Alquiler>
{

    public static readonly Error Overlap =
    MakeError(
        nameof(Overlap),
        "El Alquiler esta siendo tomado por 2 o mas clientes al mismo tiempo en la misma fecha"
    );

    public static readonly Error NotReserved =
    MakeError(
        nameof(NotReserved),
        "El alquiler no esta reservado"
    );

    public static readonly Error NotConfirmado =
    MakeError(
        nameof(NotConfirmado),
        "El alquiler no esta confirmado"
    );

    public static readonly Error AlreadyStarted =
    MakeError(
        nameof(AlreadyStarted),
        "El alquiler ya ha comenzado"
    );

}
