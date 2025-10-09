using System;
using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Reviews;

public class Review : Entity
{

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

}
