using System;
using CleanArchitecture.Domain.Alquileres;
using CleanArchitecture.Domain.Reviews;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Domain.Vehiculos;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Abstractions.DataAccess;

public interface IApplicationDbContext
{
    public DbSet<Alquiler> Alquileres { get; }
    public DbSet<Review> Reviews { get; }
    public DbSet<Vehiculo> Vehiculos { get; }
    public DbSet<User> Users { get; }


}
