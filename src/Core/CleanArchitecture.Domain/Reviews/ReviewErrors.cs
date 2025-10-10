using System;
using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Reviews;

public static class ReviewErrors
{

    public static Error NotElegible => Error.MakeError<Review>(
        nameof(NotElegible),
         "Este review y calificacion para el auto no es elegible por que aun no se completa el alquiler"
    );

}
