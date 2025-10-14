using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Reviews.Events;

namespace CleanArchitecture.Domain.Reviews;

public class Review : Entity
{

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Review() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    private Review(
        Guid id,
        Guid vehiculoId,
        Guid alquilerId,
        Guid userId,
        Rating rating,
        Comentario comentario,
        DateTime fechaCreacion
        ) : base(id)
    {
        VehiculoId = vehiculoId;
        AlquilerId = alquilerId;
        UserId = userId;
        Rating = rating;
        Comentario = comentario;
        FechaCreacion = fechaCreacion;
    }

    public Guid VehiculoId { get; private set; }
    public Guid AlquilerId { get; private set; }
    public Guid UserId { get; private set; }
    public Rating Rating { get; private set; }
    public Comentario Comentario { get; private set; }
    public DateTime FechaCreacion { get; private set; }


    public static Result<Review> Create(
        Alquiler alquiler,
        Rating rating,
        Comentario comentario,
        DateTime fechaCreacion
    )
    {
        if (alquiler.Status != AlquilerStatus.Completado)
        {
            return Result.Failure<Review>(ReviewErrors.NotElegible);
        }

        var review = new Review(
                Guid.NewGuid(),
                alquiler.VehiculoId,
                alquiler.Id,
                alquiler.UserId,
                rating,
                comentario,
                fechaCreacion
            );

        review.RaiseDomainEvent(new ReviewCreatedDomainEvent(review.Id));

        return Result.Success(review);

    }

}
