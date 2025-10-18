using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Reviews;

public sealed class ReviewErrors : EntityErrors<Review>
{

    public static Error NotElegible => MakeError(
        nameof(NotElegible),
         "Este review y calificacion para el auto no es elegible por que aun no se completa el alquiler"
    );

}
