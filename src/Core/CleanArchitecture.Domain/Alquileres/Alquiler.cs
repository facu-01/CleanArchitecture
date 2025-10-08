using System;
using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Alquileres;

public class Alquiler : Entity
{

    private Alquiler(Guid id) : base(id)
    {

    }

    public Guid VehiculoId { get; private set; }

    public Guid UserId { get; private set; }

    public DateRange Periodo { get; private set; }
     

}
